namespace SGA_Plataforma.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using SGA_Plataforma.Infrastructure.Data;
using SGA_Plataforma.Infrastructure.Models;


public interface ITournamentRepository : ICrudRepository<Tournament> {}

public sealed class TournamentRepository : ITournamentRepository 
{
    private readonly AppDbContext _context;
    private readonly DbSet<Tournament> _dbSet;

    public TournamentRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Tournament>();
    }

    public async Task<IReadOnlyList<Tournament>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .OrderBy(entity => entity.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Tournament?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<Tournament> CreateAsync(Tournament entity, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        entity.CreatedAt = now;
        entity.UpdatedAt = now;

        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Tournament?> UpdateAsync(int id, Tournament entity, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _dbSet.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (existingEntity is null)
        {
            return null;
        }

        var createdAt = existingEntity.CreatedAt;
        _context.Entry(existingEntity).CurrentValues.SetValues(entity);
        existingEntity.Id = id;
        existingEntity.CreatedAt = createdAt;
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return existingEntity;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _dbSet.FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (existingEntity is null)
        {
            return false;
        }

        _dbSet.Remove(existingEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
