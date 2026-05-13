namespace SGA_Plataforma.Api.Services;
using Microsoft.EntityFrameworkCore;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IMatchEventService : ICrudService<MatchEvent> {}

public sealed class MatchEventService : IMatchEventService
{
    private readonly IMatchEventRepository _repository;

    public MatchEventService(IMatchEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<MatchEvent>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<MatchEvent>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<MatchEvent>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<MatchEvent>.Fail($"{nameof(MatchEvent)} com id {id} nao encontrado.")
            : CustomResponse<MatchEvent>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<MatchEvent>> CreateAsync(MatchEvent entity, CancellationToken cancellationToken = default)
    {
        try
        {
            var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
            return CustomResponse<MatchEvent>.SuccessTrade(createdEntity);
        }
        catch (DbUpdateException exception) when (exception.InnerException?.Message.Contains("provider_event_id", StringComparison.OrdinalIgnoreCase) == true)
        {
            return CustomResponse<MatchEvent>.Fail("Evento duplicado para provider/provider_event_id.");
        }
    }

    public async Task<CustomResponse<MatchEvent>> UpdateAsync(int id, MatchEvent entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<MatchEvent>.Fail($"{nameof(MatchEvent)} com id {id} nao encontrado.")
            : CustomResponse<MatchEvent>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(MatchEvent)} com id {id} nao encontrado.");
    }
}
