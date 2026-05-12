using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Rounds")]
public class Round : BaseEntity
{
    [Column("match_map_id")]
    public int MatchMapId { get; set; }

    [Column("round_number")]
    public int RoundNumber { get; set; }

    [Column("winning_team_id")]
    public int? WinningTeamId { get; set; }

    [Column("winner_type")]
    public string? WinnerType { get; set; }

    [Column("started_at")]
    public DateTime? StartedAt { get; set; }

    [Column("finished_at")]
    public DateTime? FinishedAt { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(MatchMapId))]
    public MatchMap? MatchMap { get; set; }

    [ForeignKey(nameof(WinningTeamId))]
    public Team? WinningTeam { get; set; }
}