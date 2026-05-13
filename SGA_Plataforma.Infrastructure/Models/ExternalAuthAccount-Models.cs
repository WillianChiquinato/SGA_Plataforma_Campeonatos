using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("External_Auth_Account")]
public class ExternalAuthAccount : BaseEntity
{
    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("provider")]
    public string Provider { get; set; } = null!;

    [Required]
    [Column("external_account_id")]
    public string ExternalAccountId { get; set; } = null!;

    [Required]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("display_name")]
    public string? DisplayName { get; set; }

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    [Column("last_login_at")]
    public DateTime? LastLoginAt { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
}
