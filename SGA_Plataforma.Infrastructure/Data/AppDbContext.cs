using Microsoft.EntityFrameworkCore;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameAccount> GameAccounts => Set<GameAccount>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<MatchesTeams> MatchTeams => Set<MatchesTeams>();
    public DbSet<MatchEvent> MatchEvents => Set<MatchEvent>();
    public DbSet<MatchMap> MatchMaps => Set<MatchMap>();
    public DbSet<MatchLineup> MatchLineups => Set<MatchLineup>();
    public DbSet<MatchLineupPlayer> MatchLineupPlayers => Set<MatchLineupPlayer>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<PlayerMatchStats> PlayerMatchStats => Set<PlayerMatchStats>();
    public DbSet<User> PlatformUsers => Set<User>();
    public DbSet<ExternalAuthAccount> ExternalAuthAccounts => Set<ExternalAuthAccount>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Round> Rounds => Set<Round>();
    public DbSet<Stage> Stages => Set<Stage>();
    public DbSet<Status> Statuses => Set<Status>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMatchStats> TeamMatchStats => Set<TeamMatchStats>();
    public DbSet<TeamParticipant> TeamParticipants => Set<TeamParticipant>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<TournamentTeam> TournamentTeams => Set<TournamentTeam>();
    public DbSet<Models.Type> Types => Set<Models.Type>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GameAccount>()
            .HasIndex(gameAccount => new { gameAccount.Provider, gameAccount.ExternalAccountId })
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(platformUser => platformUser.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(platformUser => platformUser.Login)
            .IsUnique();

        modelBuilder.Entity<ExternalAuthAccount>()
            .HasIndex(externalAuthAccount => new { externalAuthAccount.Provider, externalAuthAccount.ExternalAccountId })
            .IsUnique();

        modelBuilder.Entity<ExternalAuthAccount>()
            .HasIndex(externalAuthAccount => externalAuthAccount.UserId);

        modelBuilder.Entity<TeamParticipant>()
            .HasIndex(teamParticipant => new { teamParticipant.TeamId, teamParticipant.PlayerId, teamParticipant.JoinedAt });

        modelBuilder.Entity<TournamentTeam>()
            .HasIndex(tournamentTeam => new { tournamentTeam.TournamentId, tournamentTeam.TeamId })
            .IsUnique();

        modelBuilder.Entity<MatchesTeams>()
            .HasIndex(matchTeam => new { matchTeam.MatchId, matchTeam.TeamId })
            .IsUnique();

        modelBuilder.Entity<MatchLineup>()
            .HasIndex(matchLineup => new { matchLineup.MatchId, matchLineup.TeamId, matchLineup.MatchMapId });

        modelBuilder.Entity<MatchLineupPlayer>()
            .HasIndex(matchLineupPlayer => new { matchLineupPlayer.MatchLineupId, matchLineupPlayer.PlayerId })
            .IsUnique();

        modelBuilder.Entity<MatchEvent>()
            .HasIndex(matchEvent => new { matchEvent.Provider, matchEvent.ProviderEventId })
            .IsUnique()
            .HasFilter("provider_event_id IS NOT NULL");
    }
}
