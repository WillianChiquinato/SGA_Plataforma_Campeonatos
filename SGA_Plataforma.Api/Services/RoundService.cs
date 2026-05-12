namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IRoundService : ICrudService<Round> {}

public sealed class RoundService : IRoundService
{
    private readonly IRoundRepository _repository;

    public RoundService(IRoundRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<Round>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<Round>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<Round>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<Round>.Fail($"{nameof(Round)} com id {id} nao encontrado.")
            : CustomResponse<Round>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<Round>> CreateAsync(Round entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<Round>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<Round>> UpdateAsync(int id, Round entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<Round>.Fail($"{nameof(Round)} com id {id} nao encontrado.")
            : CustomResponse<Round>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(Round)} com id {id} nao encontrado.");
    }
}
