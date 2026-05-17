using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class MatchesController : CrudControllerBase<Match>
{
    private readonly IMatchService _matchService;

    public MatchesController(ICrudService<Match> service, IMatchService matchService) : base(service)
    {
        _matchService = matchService;
    }

    [HttpGet]
    [Route("GetMatchesByTournamentId/{tournamentId}")]
    public async Task<ActionResult> GetMatchesByTournamentId(int tournamentId, CancellationToken cancellationToken)
    {
        var matches = await _matchService.GetMatchesByTournamentId(tournamentId, cancellationToken);
        return Ok(matches);
    }
}
