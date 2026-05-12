namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IPlayerMatchStatsService : ICrudService<PlayerMatchStats> {}

public sealed class PlayerMatchStatsService : IPlayerMatchStatsService
{
    private readonly IPlayerMatchStatsRepository _repository;

    public PlayerMatchStatsService(IPlayerMatchStatsRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<PlayerMatchStats>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<PlayerMatchStats>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<PlayerMatchStats>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<PlayerMatchStats>.Fail($"{nameof(PlayerMatchStats)} com id {id} nao encontrado.")
            : CustomResponse<PlayerMatchStats>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<PlayerMatchStats>> CreateAsync(PlayerMatchStats entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<PlayerMatchStats>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<PlayerMatchStats>> UpdateAsync(int id, PlayerMatchStats entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<PlayerMatchStats>.Fail($"{nameof(PlayerMatchStats)} com id {id} nao encontrado.")
            : CustomResponse<PlayerMatchStats>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(PlayerMatchStats)} com id {id} nao encontrado.");
    }
}
