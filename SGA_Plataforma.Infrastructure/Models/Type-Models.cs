using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Type")]
public class Type : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;
}