namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface ITeamMatchStatsService : ICrudService<TeamMatchStats> {}

public sealed class TeamMatchStatsService : ITeamMatchStatsService
{
    private readonly ITeamMatchStatsRepository _repository;

    public TeamMatchStatsService(ITeamMatchStatsRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<TeamMatchStats>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<TeamMatchStats>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<TeamMatchStats>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<TeamMatchStats>.Fail($"{nameof(TeamMatchStats)} com id {id} nao encontrado.")
            : CustomResponse<TeamMatchStats>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<TeamMatchStats>> CreateAsync(TeamMatchStats entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<TeamMatchStats>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<TeamMatchStats>> UpdateAsync(int id, TeamMatchStats entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<TeamMatchStats>.Fail($"{nameof(TeamMatchStats)} com id {id} nao encontrado.")
            : CustomResponse<TeamMatchStats>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(TeamMatchStats)} com id {id} nao encontrado.");
    }
}
