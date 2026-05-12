using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Team")]
public class Team : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("tag")]
    public string? Tag { get; set; }

    [Column("logo_url")]
    public string? LogoUrl { get; set; }
}
