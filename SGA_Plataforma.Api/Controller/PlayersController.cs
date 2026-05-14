using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class PlayersController : CrudControllerBase<Player>
{
    private readonly IPlayerService _playerService;

    public PlayersController(IPlayerService playerService) : base(playerService)
    {
        _playerService = playerService;
    }

    [HttpGet]
    [Route("GetPlayerByUserId")]
    public async Task<IActionResult> GetPlayerByUserId([FromQuery] int userId)
    {
        var playerUser = await _playerService.GetUserPlayerByIdAsync(userId);

        return playerUser.Result != null ? Ok(playerUser) : BadRequest(playerUser);
    }
}
