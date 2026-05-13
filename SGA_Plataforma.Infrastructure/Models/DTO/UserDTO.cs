namespace SGA_Plataforma.Infrastructure.Models;

public class UserCreateDTO
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Login { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; }
}

public class UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Login { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}