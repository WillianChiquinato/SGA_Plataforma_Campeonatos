using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class TournamentTeamsController : CrudControllerBase<TournamentTeam>
{
    public TournamentTeamsController(ICrudService<TournamentTeam> service) : base(service) { }
}
