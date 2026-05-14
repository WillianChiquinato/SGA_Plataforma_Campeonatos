using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGA_Plataforma.Infrastructure.Models;

public abstract class BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public static class UserSafeDTO
{
    public static UserDTO ToSafeDto(this User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Login,
            Login = user.Login,
            Role = user.Role.GetDisplayName(),
            IsActive = user.IsActive,
            LastLoginAt = user.LastLoginAt
        };
    }
}
