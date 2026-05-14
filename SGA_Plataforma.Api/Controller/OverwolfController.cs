using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Contracts.Overwolf;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;
using SGA_Plataforma.Infrastructure.Utils;

namespace SGA_Plataforma.Api.Controller;

[ApiController]
[Route("overwolf")]
public sealed class OverwolfController : ControllerBase
{
    private readonly IOverWolfService _overWolfService;

    public OverwolfController(
        IOverWolfService overWolfService)
    {
        _overWolfService = overWolfService;
    }

    [HttpGet("auth/session")]
    [Authorize]
    public async Task<IActionResult> GetSession([FromQuery] string deviceId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
            return BadRequest(new { success = false, errors = new[] { "deviceId e obrigatorio." } });

        var authenticatedUserId = User.GetAuthenticatedUserId();
        if (authenticatedUserId is null)
            return Unauthorized(new { success = false, errors = new[] { "Sessao web invalida." } });

        var response = await _overWolfService.IssueDeviceSessionAsync(authenticatedUserId.Value, deviceId.Trim(), cancellationToken);
        return ToActionResult(response);
    }

    [HttpPost("link")]
    [Authorize]
    public async Task<IActionResult> Link([FromBody] OverwolfLinkRequest request, CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.DeviceId))
            return BadRequest(new { success = false, errors = new[] { "deviceId e obrigatorio." } });

        var authenticatedUserId = User.GetAuthenticatedUserId();
        if (authenticatedUserId is null)
            return Unauthorized(new { success = false, errors = new[] { "Sessao web invalida." } });

        var response = await _overWolfService.IssueDeviceSessionAsync(authenticatedUserId.Value, request.DeviceId.Trim(), cancellationToken);
        return ToActionResult(response);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] OverwolfRefreshRequest request, CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.DeviceId) || string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest(new { success = false, errors = new[] { "deviceId e refreshToken sao obrigatorios." } });

        var cachedPayload = await _overWolfService.GetCachedSessionAsync(request.RefreshToken.Trim(), cancellationToken);
        if (string.IsNullOrWhiteSpace(cachedPayload))
        {
            return Unauthorized(new { success = false, errors = new[] { "Refresh token invalido ou expirado." } });
        }

        var session = JsonSerializer.Deserialize<OverwolfDeviceSession>(cachedPayload);
        if (session is null
            || !string.Equals(session.DeviceId, request.DeviceId.Trim(), StringComparison.Ordinal)
            || session.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            return Unauthorized(new { success = false, errors = new[] { "Refresh token invalido ou expirado." } });
        }

        var revokeResponse = await _overWolfService.RevokeDeviceSessionAsync(session.DeviceId, cancellationToken);
        if (!revokeResponse.Success)
            return BadRequest(revokeResponse);

        var response = await _overWolfService.IssueDeviceSessionAsync(session.UserId, session.DeviceId, cancellationToken);
        return ToActionResult(response);
    }

    private IActionResult ToActionResult(CustomResponse<OverwolfSessionResponse> response)
    {
        return response.Success ? Ok(response) : BadRequest(response);
    }
}