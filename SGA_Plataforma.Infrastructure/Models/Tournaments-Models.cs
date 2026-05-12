using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Tournament")]
public class Tournament : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("banner_url")]
    public string? BannerUrl { get; set; }

    [Column("start_date")]
    public DateTime? StartDate { get; set; }

    [Column("end_date")]
    public DateTime? EndDate { get; set; }

    [Column("created_by")]
    public string? CreatedBy { get; set; }

    [Column("format")]
    public string? Format { get; set; }

    [Column("bracket_type")]
    public string? BracketType { get; set; }

    [Column("max_teams")]
    public int? MaxTeams { get; set; }

    [Column("organizer")]
    public string? Organizer { get; set; }

    [Column("rulebook_url")]
    public string? RulebookUrl { get; set; }

    [Column("prize_pool")]
    public decimal? PrizePool { get; set; }

    [Column("region")]
    public string? Region { get; set; }

    [Column("timezone")]
    public string? Timezone { get; set; }

    [Column("patch_version")]
    public string? PatchVersion { get; set; }

    [Column("roster_lock_at")]
    public DateTime? RosterLockAt { get; set; }

    [Column("status_id")]
    public int? StatusId { get; set; }

    [Column("game_id")]
    public int? GameId { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }

    [ForeignKey(nameof(StatusId))]
    public Status? Status { get; set; }
}