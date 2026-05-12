namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IMatchLineupService : ICrudService<MatchLineup> {}

public sealed class MatchLineupService : IMatchLineupService
{
    private readonly IMatchLineupRepository _repository;

    public MatchLineupService(IMatchLineupRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<MatchLineup>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<MatchLineup>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<MatchLineup>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<MatchLineup>.Fail($"{nameof(MatchLineup)} com id {id} nao encontrado.")
            : CustomResponse<MatchLineup>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<MatchLineup>> CreateAsync(MatchLineup entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<MatchLineup>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<MatchLineup>> UpdateAsync(int id, MatchLineup entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<MatchLineup>.Fail($"{nameof(MatchLineup)} com id {id} nao encontrado.")
            : CustomResponse<MatchLineup>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(MatchLineup)} com id {id} nao encontrado.");
    }
}
