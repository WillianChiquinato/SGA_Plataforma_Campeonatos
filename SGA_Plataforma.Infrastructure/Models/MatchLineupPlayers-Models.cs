using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Match_Lineup_Players")]
public class MatchLineupPlayer : BaseEntity
{
    [Column("match_lineup_id")]
    public int MatchLineupId { get; set; }

    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("game_account_id")]
    public int? GameAccountId { get; set; }

    [Column("role_id")]
    public int? RoleId { get; set; }

    [Column("is_starter")]
    public bool IsStarter { get; set; } = true;

    [Column("is_captain")]
    public bool IsCaptain { get; set; }

    [Column("is_substitute")]
    public bool IsSubstitute { get; set; }

    [ForeignKey(nameof(MatchLineupId))]
    public MatchLineup? MatchLineup { get; set; }

    [ForeignKey(nameof(PlayerId))]
    public Player? Player { get; set; }

    [ForeignKey(nameof(GameAccountId))]
    public GameAccount? GameAccount { get; set; }

    [ForeignKey(nameof(RoleId))]
    public Role? Role { get; set; }
}