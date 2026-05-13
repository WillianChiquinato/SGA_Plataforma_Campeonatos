using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Utils;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly IUserService _userService;

    public AuthController(TokenService tokenService, IUserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost]
    [AllowAnonymous]
    [EnableRateLimiting("loginPolicy")]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.GetUserByEmailAndPassword(request.Email!, request.Password!);

        if (user.Errors.Count > 0)
        {
            return BadRequest(user.Errors);
        }

        var token = _tokenService.GenerateToken(user.Result);
        _tokenService.AppendAuthCookie(Response, token, Request.IsHttps);

        return Ok(new
        {
            token,
            user = user.Result
        });
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("Logout")]
    public IActionResult Logout()
    {
        _tokenService.DeleteAuthCookie(Response, Request.IsHttps);

        return Ok(new
        {
            success = true,
            errors = Array.Empty<string>(),
            result = "Logout realizado com sucesso."
        });
    }

    [HttpGet]
    [Authorize]
    [Route("Logged")]
    public async Task<IActionResult> Logged()
    {
        var authenticatedUserId = User.GetAuthenticatedUserId();
        if (authenticatedUserId is null)
            return Unauthorized();

        var user = await _userService.GetByIdAsync(authenticatedUserId.Value);
        if (!user.Success || user.Result == null)
            return Unauthorized();

        return Ok(new
        {
            success = true,
            errors = Array.Empty<string>(),
            result = user.Result.ToSafeDto()
        });
    }
}