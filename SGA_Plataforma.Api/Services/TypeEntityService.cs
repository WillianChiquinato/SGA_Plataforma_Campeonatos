namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;
using TypeEntity = SGA_Plataforma.Infrastructure.Models.Type;

public interface ITypeEntityService : ICrudService<TypeEntity> {}

public sealed class TypeEntityService : ITypeEntityService
{
    private readonly ITypeEntityRepository _repository;

    public TypeEntityService(ITypeEntityRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<TypeEntity>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<TypeEntity>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<TypeEntity>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<TypeEntity>.Fail($"{nameof(TypeEntity)} com id {id} nao encontrado.")
            : CustomResponse<TypeEntity>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<TypeEntity>> CreateAsync(TypeEntity entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<TypeEntity>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<TypeEntity>> UpdateAsync(int id, TypeEntity entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<TypeEntity>.Fail($"{nameof(TypeEntity)} com id {id} nao encontrado.")
            : CustomResponse<TypeEntity>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(TypeEntity)} com id {id} nao encontrado.");
    }
}
