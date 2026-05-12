using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Stage")]
public class Stage : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("tournament_id")]
    public int TournamentId { get; set; }

    [Column("type_id")]
    public int? TypeId { get; set; }

    [Column("sort_order")]
    public int SortOrder { get; set; }

    [Column("starts_at")]
    public DateTime? StartsAt { get; set; }

    [Column("ends_at")]
    public DateTime? EndsAt { get; set; }

    [Column("advancement_rules")]
    public string? AdvancementRules { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(TournamentId))]
    public Tournament? Tournament { get; set; }

    [ForeignKey(nameof(TypeId))]
    public Type? Type { get; set; }
}