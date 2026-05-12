using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;

namespace SGA_Plataforma.Api.Services;

public interface ICrudService<TEntity> where TEntity : BaseEntity
{
    Task<CustomResponse<IReadOnlyList<TEntity>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CustomResponse<TEntity>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CustomResponse<TEntity>> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<CustomResponse<TEntity>> UpdateAsync(int id, TEntity entity, CancellationToken cancellationToken = default);
    Task<CustomResponse<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
}