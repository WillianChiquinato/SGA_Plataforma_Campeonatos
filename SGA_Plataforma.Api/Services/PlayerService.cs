namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IPlayerService : ICrudService<Player>
{
    Task<CustomResponse<UserPlayerDTO>> GetUserPlayerByIdAsync(int id, CancellationToken cancellationToken = default);
}

public sealed class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _repository;

    public PlayerService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<Player>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<Player>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<Player>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<Player>.Fail($"{nameof(Player)} com id {id} nao encontrado.")
            : CustomResponse<Player>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<Player>> CreateAsync(Player entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<Player>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<Player>> UpdateAsync(int id, Player entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<Player>.Fail($"{nameof(Player)} com id {id} nao encontrado.")
            : CustomResponse<Player>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(Player)} com id {id} nao encontrado.");
    }

    public async Task<CustomResponse<UserPlayerDTO>> GetUserPlayerByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _repository.GetUserPlayerByIdAsync(id, cancellationToken);

            return result is null
                ? CustomResponse<UserPlayerDTO>.Fail($"{nameof(User)} com id {id} nao encontrado.")
                : CustomResponse<UserPlayerDTO>.SuccessTrade(result);
        }
        catch (Exception ex)
        {
            return CustomResponse<UserPlayerDTO>.Fail($"Erro ao obter usuário: {ex.Message}");
        }
    }
}
