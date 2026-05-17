namespace SGA_Plataforma.Infrastructure.Models;

public class MatchTeamListDTO
{
    public int Id { get; set; }
    public int TournamentId { get; set; }
    public int? BestOf { get; set; }
    public string? BracketPosition { get; set; }
    public DateTime? StartedAt { get; set; }

    public List<MatchTeamDTO> Teams { get; set; } = new();
}

public class MatchTeamDTO
{
    public int TeamId { get; set; }
    public string Side { get; set; } = null!;
    public int? Score { get; set; }
    public bool IsWinner { get; set; }

    public TeamDTO Team { get; set; } = null!;
}

public class TeamDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? LogoUrl { get; set; }
    public bool IsPublic { get; set; }
    public List<PlayerDTO> Players { get; set; } = new();
}