namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IStageService : ICrudService<Stage> {}

public sealed class StageService : IStageService
{
    private readonly IStageRepository _repository;

    public StageService(IStageRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<Stage>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<Stage>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<Stage>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<Stage>.Fail($"{nameof(Stage)} com id {id} nao encontrado.")
            : CustomResponse<Stage>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<Stage>> CreateAsync(Stage entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<Stage>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<Stage>> UpdateAsync(int id, Stage entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<Stage>.Fail($"{nameof(Stage)} com id {id} nao encontrado.")
            : CustomResponse<Stage>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(Stage)} com id {id} nao encontrado.");
    }
}
