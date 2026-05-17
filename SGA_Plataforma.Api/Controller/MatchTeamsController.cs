using Microsoft.AspNetCore.Mvc;
using SGA_Plataforma.Api.Services;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Api.Controller;

[Route("api/[controller]")]
public sealed class MatchTeamsController : CrudControllerBase<MatchesTeams>
{
    private readonly IMatchesTeamsService _matchesTeamsService;

    public MatchTeamsController(IMatchesTeamsService matchesTeamsService, ICrudService<MatchesTeams> service) : base(service)
    {
        _matchesTeamsService = matchesTeamsService;
    }

    [HttpGet]
    [Route("GetTeamsByMatchAndTeamId")]
    public async Task<ActionResult> GetTeamsByMatchAndTeamId(int matchId, int teamId, CancellationToken cancellationToken)
    {
        var matchTeams = await _matchesTeamsService.GetTeamsByMatchAndTeamId(matchId, teamId, cancellationToken);
        return Ok(matchTeams);
    }
}
