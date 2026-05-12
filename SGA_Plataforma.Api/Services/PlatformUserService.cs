namespace SGA_Plataforma.Api.Services;
using SGA_Plataforma.Api.Repositories;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;


public interface IPlatformUserService : ICrudService<PlatformUser> {}

public sealed class PlatformUserService : IPlatformUserService
{
    private readonly IPlatformUserRepository _repository;

    public PlatformUserService(IPlatformUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResponse<IReadOnlyList<PlatformUser>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return CustomResponse<IReadOnlyList<PlatformUser>>.SuccessTrade(entities, entities.Count);
    }

    public async Task<CustomResponse<PlatformUser>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        return entity is null
            ? CustomResponse<PlatformUser>.Fail($"{nameof(PlatformUser)} com id {id} nao encontrado.")
            : CustomResponse<PlatformUser>.SuccessTrade(entity);
    }

    public async Task<CustomResponse<PlatformUser>> CreateAsync(PlatformUser entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
        return CustomResponse<PlatformUser>.SuccessTrade(createdEntity);
    }

    public async Task<CustomResponse<PlatformUser>> UpdateAsync(int id, PlatformUser entity, CancellationToken cancellationToken = default)
    {
        var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
        return updatedEntity is null
            ? CustomResponse<PlatformUser>.Fail($"{nameof(PlatformUser)} com id {id} nao encontrado.")
            : CustomResponse<PlatformUser>.SuccessTrade(updatedEntity);
    }

    public async Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted
            ? CustomResponse<bool>.SuccessTrade(true)
            : CustomResponse<bool>.Fail($"{nameof(PlatformUser)} com id {id} nao encontrado.");
    }
}
