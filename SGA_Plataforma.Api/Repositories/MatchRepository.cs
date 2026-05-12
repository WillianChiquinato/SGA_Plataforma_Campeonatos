namespace SGA_Plataforma.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using SGA_Plataforma.Infrastructure.Data;
using SGA_Plataforma.Infrastructure.Models;


public interface IMatchRepository : ICrudRepository<Match> {}

public sealed class MatchRepository : IMatchRepository 
{
    private readonly AppDbContext _context;
    private readonly DbSet<Match> _dbSet;

    public MatchRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Match>();
    }

    public async Task<IReadOnlyList<Match>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .OrderBy(entity => entity.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Match?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<Match> CreateAsync(Match entity, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        entity.CreatedAt = now;
        entity.UpdatedAt = now;

        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Match?> UpdateAsync(int id, Match entity, CancellationToken cancellationToken = default)
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
