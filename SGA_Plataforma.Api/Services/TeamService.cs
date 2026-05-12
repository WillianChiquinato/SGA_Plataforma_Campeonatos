namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface ITeamService : ICrudService<Team> {}

public sealed class TeamService : ITeamService
{
    private readonly ITeamRepository _repository;

    public TeamService(ITeamRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<Team>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<Team>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<Team>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<Team>.Fail($"{nameof(Team)} com id {id} nao encontrado.")
            : CustomResponse<Team>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<Team>> CreateAsync(Team entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<Team>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<Team>> UpdateAsync(int id, Team entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<Team>.Fail($"{nameof(Team)} com id {id} nao encontrado.")
            : CustomResponse<Team>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(Team)} com id {id} nao encontrado.");
    }
}
