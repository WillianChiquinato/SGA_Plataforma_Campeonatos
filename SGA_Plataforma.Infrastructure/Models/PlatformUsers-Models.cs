using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SGA_Plataforma.Infrastructure.Models;

[Table("Platform_User")]
public class PlatformUser : BaseEntity
{
    [Required]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("login")]
    public string Login { get; set; } = null!;

    [Required]
    [Column("password_hash")]
    [JsonIgnore]
    public string PasswordHash { get; set; } = null!;

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("last_login_at")]
    public DateTime? LastLoginAt { get; set; }
}