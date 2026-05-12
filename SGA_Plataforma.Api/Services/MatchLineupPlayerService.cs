namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IMatchLineupPlayerService : ICrudService<MatchLineupPlayer> {}

public sealed class MatchLineupPlayerService : IMatchLineupPlayerService
{
    private readonly IMatchLineupPlayerRepository _repository;

    public MatchLineupPlayerService(IMatchLineupPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<MatchLineupPlayer>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<MatchLineupPlayer>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<MatchLineupPlayer>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<MatchLineupPlayer>.Fail($"{nameof(MatchLineupPlayer)} com id {id} nao encontrado.")
            : CustomResponse<MatchLineupPlayer>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<MatchLineupPlayer>> CreateAsync(MatchLineupPlayer entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<MatchLineupPlayer>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<MatchLineupPlayer>> UpdateAsync(int id, MatchLineupPlayer entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<MatchLineupPlayer>.Fail($"{nameof(MatchLineupPlayer)} com id {id} nao encontrado.")
            : CustomResponse<MatchLineupPlayer>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(MatchLineupPlayer)} com id {id} nao encontrado.");
    }
}
