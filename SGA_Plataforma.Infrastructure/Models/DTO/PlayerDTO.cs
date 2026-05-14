namespace SGA_Plataforma.Infrastructure.Models;

public class PlayerDTO
{
    public int Id { get; set; }
    public string Nickname { get; set; } = null!;
    public string AvatarUrl { get; set; } = null!;
    public bool IsProfilePublic { get; set; }
}