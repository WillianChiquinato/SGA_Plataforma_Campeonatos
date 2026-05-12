using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Games")]
public class Game : BaseEntity
{
    [Required]
    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("name")]
    public string? Name { get; set; }
}