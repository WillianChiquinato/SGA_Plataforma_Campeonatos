namespace SGA_Plataforma.Api.Repositories;

using Microsoft.EntityFrameworkCore;
using SGA_Plataforma.Infrastructure.Data;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;

public interface IMatchRepository : ICrudRepository<Match>
{
    Task<List<MatchTeamListDTO>> GetMatchesByTournamentId(int tournamentId, CancellationToken cancellationToken = default);
}

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

    public async Task<List<MatchTeamListDTO>> GetMatchesByTournamentId(int tournamentId,
    CancellationToken cancellationToken = default)
    {
        var matches = await _dbSet
            .Where(m => m.TournamentId == tournamentId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var matcheTeams = await _context.MatchTeams
            .Where(mt => matches.Select(m => m.Id).Contains(mt.MatchId))
            .Include(mt => mt.Team)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var result = matches.Select(match => new MatchTeamListDTO
        {
            Id = match.Id,
            TournamentId = match.TournamentId,
            BestOf = match.BestOf,
            BracketPosition = match.BracketPosition,
            StartedAt = match.StartedAt,

            Teams = matcheTeams
                .Where(mt => mt.MatchId == match.Id)
                .Select(mt => new MatchTeamDTO
                {
                    TeamId = mt.TeamId,
                    Side = mt.Side ?? "",
                    Score = mt.Score,
                    IsWinner = mt.IsWinner,

                Team = new TeamDTO
                {
                    Id = mt.Team!.Id,
                    Name = mt.Team.Name,
                    LogoUrl = mt.Team.LogoUrl,
                }
            }).ToList()
        }).ToList();

        return result;
    }
}
