namespace SGA_Plataforma.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using SGA_Plataforma.Infrastructure.Data;
using SGA_Plataforma.Infrastructure.Models;


public interface IPlayerRepository : ICrudRepository<Player>
{
    Task<UserPlayerDTO?> GetUserPlayerByIdAsync(int id, CancellationToken cancellationToken = default);
}

public sealed class PlayerRepository : IPlayerRepository 
{
    private readonly AppDbContext _context;
    private readonly DbSet<Player> _dbSet;

    public PlayerRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Player>();
    }

    public async Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .OrderBy(entity => entity.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Player?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<Player> CreateAsync(Player entity, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        entity.CreatedAt = now;
        entity.UpdatedAt = now;

        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Player?> UpdateAsync(int id, Player entity, CancellationToken cancellationToken = default)
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

    public async Task<UserPlayerDTO?> GetUserPlayerByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(player => player.UserId == id)
            .Select(player => new UserPlayerDTO
            {
                Id = player.User!.Id,
                Email = player.User.Email,
                Login = player.User.Login,
                Role = player.User.Role == UserRole.Admin
                    ? "Administrador"
                    : player.User.Role == UserRole.PlatformUser
                        ? "Jogador"
                        : "Desconhecido",
                IsActive = player.User.IsActive,
                LastLoginAt = player.User.LastLoginAt,
                Player = new PlayerDTO
                {
                    Id = player.Id,
                    Nickname = player.Name,
                    AvatarUrl = player.AvatarUrl ?? string.Empty,
                    IsProfilePublic = player.IsProfilePublic
                }
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

}
