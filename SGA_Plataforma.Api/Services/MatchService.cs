namespace SGA_Plataforma.Api.Services;

using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IMatchService : ICrudService<Match>
{
    Task<CustomResponse<List<MatchTeamListDTO>>> GetMatchesByTournamentId(int tournamentId, CancellationToken cancellationToken = default);
}

public sealed class MatchService : IMatchService
{
    private readonly IMatchRepository _repository;

    public MatchService(IMatchRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<Match>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<Match>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<Match>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<Match>.Fail($"{nameof(Match)} com id {id} nao encontrado.")
            : CustomResponse<Match>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<Match>> CreateAsync(Match entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<Match>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<Match>> UpdateAsync(int id, Match entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<Match>.Fail($"{nameof(Match)} com id {id} nao encontrado.")
            : CustomResponse<Match>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(Match)} com id {id} nao encontrado.");
    }

    public async Task<CustomResponse<List<MatchTeamListDTO>>> GetMatchesByTournamentId(
    int tournamentId,
    CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetMatchesByTournamentId(tournamentId, cancellationToken);

        return CustomResponse<List<MatchTeamListDTO>>
            .SuccessTrade(result);
    }
}
