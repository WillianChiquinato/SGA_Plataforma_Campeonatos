namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IMatchesTeamsService : ICrudService<MatchesTeams>
{
    Task<CustomResponse<IReadOnlyList<MatchesTeams>>> GetTeamsByMatchAndTeamId(int matchId, int teamId, CancellationToken cancellationToken = default);
}

public sealed class MatchesTeamsService : IMatchesTeamsService
{
    private readonly IMatchesTeamsRepository _repository;

    public MatchesTeamsService(IMatchesTeamsRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<MatchesTeams>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<MatchesTeams>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<MatchesTeams>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<MatchesTeams>.Fail($"{nameof(MatchesTeams)} com id {id} nao encontrado.")
            : CustomResponse<MatchesTeams>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<MatchesTeams>> CreateAsync(MatchesTeams entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<MatchesTeams>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<MatchesTeams>> UpdateAsync(int id, MatchesTeams entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<MatchesTeams>.Fail($"{nameof(MatchesTeams)} com id {id} nao encontrado.")
            : CustomResponse<MatchesTeams>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(MatchesTeams)} com id {id} nao encontrado.");
    }

    public async Task<CustomResponse<IReadOnlyList<MatchesTeams>>> GetTeamsByMatchAndTeamId(int matchId, int teamId, CancellationToken cancellationToken = default)
    {
        var matchTeams = await _repository.GetTeamsByMatchAndTeamId(matchId, teamId, cancellationToken);
        return CustomResponse<IReadOnlyList<MatchesTeams>>.SuccessTrade(matchTeams, matchTeams.Count);
    }
}
