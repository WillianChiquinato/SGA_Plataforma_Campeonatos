using System.Text.Json;
using SGA_Plataforma.Infrastructure.Models;

namespace SGA_Plataforma.Contracts.Overwolf;

public sealed class OverwolfLinkRequest
{
    public string DeviceId { get; set; } = string.Empty;
}

public sealed class OverwolfRefreshRequest
{
    public string DeviceId { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}

public sealed class OverwolfSessionResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public int AccessTokenExpiresInSeconds { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
    public string DeviceId { get; set; } = string.Empty;
    public UserPlayerDTO? User { get; set; }
    public string HubUrl { get; set; } = string.Empty;
}

public sealed class OverwolfDeviceSession
{
    public string DeviceId { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}