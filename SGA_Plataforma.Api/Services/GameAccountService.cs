namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IGameAccountService : ICrudService<GameAccount> {}

public sealed class GameAccountService : IGameAccountService
{
    private readonly IGameAccountRepository _repository;

    public GameAccountService(IGameAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<GameAccount>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<GameAccount>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<GameAccount>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<GameAccount>.Fail($"{nameof(GameAccount)} com id {id} nao encontrado.")
            : CustomResponse<GameAccount>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<GameAccount>> CreateAsync(GameAccount entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<GameAccount>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<GameAccount>> UpdateAsync(int id, GameAccount entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<GameAccount>.Fail($"{nameof(GameAccount)} com id {id} nao encontrado.")
            : CustomResponse<GameAccount>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(GameAccount)} com id {id} nao encontrado.");
    }
}
