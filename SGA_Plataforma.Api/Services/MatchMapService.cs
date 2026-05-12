namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IMatchMapService : ICrudService<MatchMap> {}

public sealed class MatchMapService : IMatchMapService
{
    private readonly IMatchMapRepository _repository;

    public MatchMapService(IMatchMapRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<MatchMap>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<MatchMap>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<MatchMap>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<MatchMap>.Fail($"{nameof(MatchMap)} com id {id} nao encontrado.")
            : CustomResponse<MatchMap>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<MatchMap>> CreateAsync(MatchMap entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<MatchMap>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<MatchMap>> UpdateAsync(int id, MatchMap entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<MatchMap>.Fail($"{nameof(MatchMap)} com id {id} nao encontrado.")
            : CustomResponse<MatchMap>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(MatchMap)} com id {id} nao encontrado.");
    }
}
