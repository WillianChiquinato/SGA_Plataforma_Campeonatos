namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface ITournamentService : ICrudService<Tournament> {}

public sealed class TournamentService : ITournamentService
{
    private readonly ITournamentRepository _repository;

    public TournamentService(ITournamentRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<Tournament>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<Tournament>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<Tournament>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<Tournament>.Fail($"{nameof(Tournament)} com id {id} nao encontrado.")
            : CustomResponse<Tournament>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<Tournament>> CreateAsync(Tournament entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<Tournament>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<Tournament>> UpdateAsync(int id, Tournament entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<Tournament>.Fail($"{nameof(Tournament)} com id {id} nao encontrado.")
            : CustomResponse<Tournament>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(Tournament)} com id {id} nao encontrado.");
    }
}
