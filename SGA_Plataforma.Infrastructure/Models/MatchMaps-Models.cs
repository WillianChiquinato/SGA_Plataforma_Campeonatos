using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Match_Maps")]
public class MatchMap : BaseEntity
{
    [Column("match_id")]
    public int MatchId { get; set; }

    [Column("map_name")]
    public string? MapName { get; set; }

    [Column("map_order")]
    public int? MapOrder { get; set; }

    [Column("status_id")]
    public int? StatusId { get; set; }

    [Column("team_1_score")]
    public int? Team1Score { get; set; }

    [Column("team_2_score")]
    public int? Team2Score { get; set; }

    [Column("winner_team_id")]
    public int? WinnerTeamId { get; set; }

    [Column("started_at")]
    public DateTime? StartedAt { get; set; }

    [Column("ended_at")]
    public DateTime? EndedAt { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(MatchId))]
    public Match? Match { get; set; }

    [ForeignKey(nameof(StatusId))]
    public Status? Status { get; set; }

    [ForeignKey(nameof(WinnerTeamId))]
    public Team? WinnerTeam { get; set; }
}