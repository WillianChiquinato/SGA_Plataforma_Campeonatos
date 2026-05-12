using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Team_Participant")]
public class TeamParticipant : BaseEntity
{
    [Required]
    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("team_id")]
    public int TeamId { get; set; }

    [Column("joined_at", TypeName = "timestamp without time zone")]
    public DateTime JoinedAt { get; set; }

    [Column("left_at", TypeName = "timestamp without time zone")]
    public DateTime? LeftAt { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("is_starter")]
    public bool IsStarter { get; set; } = true;

    [Column("is_captain")]
    public bool IsCaptain { get; set; }

    [Column("is_substitute")]
    public bool IsSubstitute { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(RoleId))]
    public Role? Role { get; set; }

    [ForeignKey(nameof(PlayerId))]
    public Player? Player { get; set; }

    [ForeignKey(nameof(TeamId))]
    public Team? Team { get; set; }
}