using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;


[Table("Player")]
public class Player : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("is_profile_public")]
    public bool IsProfilePublic { get; set; } = true;

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}