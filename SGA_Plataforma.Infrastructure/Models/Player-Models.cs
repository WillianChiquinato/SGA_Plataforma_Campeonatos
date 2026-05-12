using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;


[Table("Player")]
public class Player : BaseEntity
{
    [Required]
    [Column("name")]
    public string Name { get; set; } = null!;

    [Required]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("login")]
    public string Login { get; set; } = null!;

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("is_profile_public")]
    public bool IsProfilePublic { get; set; } = true;

    [ForeignKey(nameof(UserId))]
    public PlatformUser? User { get; set; }
}