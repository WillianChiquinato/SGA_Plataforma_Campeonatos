using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Role")]
public class Role : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("game_id")]
    public int? GameId { get; set; }

    // 🔗 Relacionamentos
    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }
}