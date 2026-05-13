using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Match_Lineups")]
public class MatchLineup : BaseEntity
{
    [Column("match_id")]
    public int MatchId { get; set; }

    [Column("team_id")]
    public int TeamId { get; set; }

    [Column("match_map_id")]
    public int? MatchMapId { get; set; }

    [Column("status")]
    public string? Status { get; set; }

    [Column("source")]
    public string? Source { get; set; }

    [Column("submitted_by_user_id")]
    public int? SubmittedByUserId { get; set; }

    [Column("locked_at")]
    public DateTime? LockedAt { get; set; }

    [ForeignKey(nameof(MatchId))]
    public Match? Match { get; set; }

    [ForeignKey(nameof(TeamId))]
    public Team? Team { get; set; }

    [ForeignKey(nameof(MatchMapId))]
    public MatchMap? MatchMap { get; set; }

    [ForeignKey(nameof(SubmittedByUserId))]
    public User? SubmittedByUser { get; set; }
}