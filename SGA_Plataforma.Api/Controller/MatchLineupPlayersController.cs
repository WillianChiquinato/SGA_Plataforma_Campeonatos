using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class MatchLineupPlayersController : CrudControllerBase<MatchLineupPlayer>
{
    public MatchLineupPlayersController(ICrudService<MatchLineupPlayer> service) : base(service) { }
}
