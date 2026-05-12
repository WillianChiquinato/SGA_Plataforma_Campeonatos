namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IGameService : ICrudService<Game> {}

public sealed class GameService : IGameService
{
    private readonly IGameRepository _repository;

    public GameService(IGameRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<Game>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<Game>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<Game>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<Game>.Fail($"{nameof(Game)} com id {id} nao encontrado.")
            : CustomResponse<Game>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<Game>> CreateAsync(Game entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<Game>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<Game>> UpdateAsync(int id, Game entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<Game>.Fail($"{nameof(Game)} com id {id} nao encontrado.")
            : CustomResponse<Game>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(Game)} com id {id} nao encontrado.");
    }
}
