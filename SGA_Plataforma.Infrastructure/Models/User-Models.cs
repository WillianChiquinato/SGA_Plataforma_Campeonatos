using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SGA_Plataforma.Infrastructure.Models;

public enum UserRole
{
    Admin = 1,
    PlatformUser = 2
}

[Table("User")]
public class User : BaseEntity
{
    [Required]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Required]
    [Column("login")]
    public string Login { get; set; } = null!;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Required]
    [Column("role")]
    public UserRole Role { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; } = true;

    [Column("last_login_at")]
    public DateTime? LastLoginAt { get; set; }
}