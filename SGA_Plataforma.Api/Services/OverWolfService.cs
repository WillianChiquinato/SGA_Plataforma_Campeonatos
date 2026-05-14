namespace SGA_Plataforma.Api.Services;

using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using SGA_Plataforma.Contracts.Overwolf;
using SGA_Plataforma.Infrastructure.Models;
using SGA_Plataforma.Infrastructure.Response;

public interface IOverWolfService
{
    Task<CustomResponse<OverwolfSessionResponse>> IssueDeviceSessionAsync(int userId, string deviceId, CancellationToken cancellationToken = default);
    Task<CustomResponse<bool>> RevokeDeviceSessionAsync(string deviceId, CancellationToken cancellationToken = default);
    Task<string?> GetCachedSessionAsync(string refreshToken, CancellationToken cancellationToken = default);
}

public sealed class OverWolfService : IOverWolfService
{
    private readonly IPlayerService _playerService;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;
    private readonly TokenService _tokenService;
    private const string DeviceSessionCachePrefix = "overwolf:device:";
    private const string RefreshSessionCachePrefix = "overwolf:refresh:";

    public OverWolfService(IPlayerService playerService, IUserService userService, IConfiguration configuration, IDistributedCache cache, TokenService tokenService )
    {
        _playerService = playerService;
        _userService = userService;
        _configuration = configuration;
        _cache = cache;
        _tokenService = tokenService;
    }

    public async Task<string?> GetCachedSessionAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _cache.GetStringAsync(BuildRefreshSessionKey(refreshToken), cancellationToken);
    }

    public async Task<CustomResponse<OverwolfSessionResponse>> IssueDeviceSessionAsync(int userId, string deviceId, CancellationToken cancellationToken = default)
    {
        var userResponse = await _userService.GetByIdAsync(userId, cancellationToken);
        if (!userResponse.Success || userResponse.Result is null)
        {
            return CustomResponse<OverwolfSessionResponse>.Fail($"Usuario com id {userId} nao encontrado.");
        }

        var playerResponse = await _playerService.GetUserPlayerByIdAsync(userId, cancellationToken);
        if (!playerResponse.Success || playerResponse.Result is null)
        {
            return CustomResponse<OverwolfSessionResponse>.Fail($"Player do usuario com id {userId} nao encontrado.");
        }

        var previousSessionPayload = await _cache.GetStringAsync(BuildDeviceSessionKey(deviceId), cancellationToken);
        if (!string.IsNullOrWhiteSpace(previousSessionPayload))
        {
            var previousSession = JsonSerializer.Deserialize<OverwolfDeviceSession>(previousSessionPayload);
            if (previousSession is not null)
            {
                await DeleteSessionAsync(previousSession.DeviceId, previousSession.RefreshToken, cancellationToken);
            }
        }

        var accessToken = _tokenService.GenerateToken(userResponse.Result.ToSafeDto());
        var accessTokenExpiresInSeconds = (int)TimeSpan.FromMinutes(GetJwtExpireMinutes()).TotalSeconds;
        var refreshExpiresAt = DateTimeOffset.UtcNow.AddDays(GetRefreshTokenDays());
        var refreshToken = GenerateSecureToken();

        var session = new OverwolfDeviceSession
        {
            DeviceId = deviceId,
            UserId = userId,
            RefreshToken = refreshToken,
            ExpiresAt = refreshExpiresAt,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await PersistSessionAsync(session, cancellationToken);

        var response = new OverwolfSessionResponse
        {
            AccessToken = accessToken,
            AccessTokenExpiresInSeconds = accessTokenExpiresInSeconds,
            RefreshToken = refreshToken,
            RefreshTokenExpiresAt = refreshExpiresAt,
            DeviceId = deviceId,
            User = playerResponse.Result,
            HubUrl = "/hubs/player"
        };
        return CustomResponse<OverwolfSessionResponse>.SuccessTrade(response);
    }

    public async Task<CustomResponse<bool>> RevokeDeviceSessionAsync(string deviceId, CancellationToken cancellationToken = default)
    {
        var previousSessionPayload = await _cache.GetStringAsync(BuildDeviceSessionKey(deviceId), cancellationToken);
        if (string.IsNullOrWhiteSpace(previousSessionPayload))
        {
            return CustomResponse<bool>.SuccessTrade(true);
        }

        var previousSession = JsonSerializer.Deserialize<OverwolfDeviceSession>(previousSessionPayload);
        if (previousSession is null)
        {
            await _cache.RemoveAsync(BuildDeviceSessionKey(deviceId), cancellationToken);
            return CustomResponse<bool>.SuccessTrade(true);
        }

        await DeleteSessionAsync(previousSession.DeviceId, previousSession.RefreshToken, cancellationToken);
        return CustomResponse<bool>.SuccessTrade(true);
    }

    private async Task PersistSessionAsync(OverwolfDeviceSession session, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Serialize(session);
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = session.ExpiresAt
        };

        await _cache.SetStringAsync(BuildDeviceSessionKey(session.DeviceId), payload, cacheOptions, cancellationToken);
        await _cache.SetStringAsync(BuildRefreshSessionKey(session.RefreshToken), payload, cacheOptions, cancellationToken);
    }

    private async Task DeleteSessionAsync(string deviceId, string refreshToken, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(BuildDeviceSessionKey(deviceId), cancellationToken);
        await _cache.RemoveAsync(BuildRefreshSessionKey(refreshToken), cancellationToken);
    }

    private int GetJwtExpireMinutes()
    {
        return _configuration.GetValue<int?>("Jwt:ExpireMinutes") ?? 60;
    }

    private int GetRefreshTokenDays()
    {
        return _configuration.GetValue<int?>("OverwolfAuth:RefreshTokenDays") ?? 30;
    }

    private static string BuildDeviceSessionKey(string deviceId)
        => $"{DeviceSessionCachePrefix}{deviceId}";

    private static string BuildRefreshSessionKey(string refreshToken)
        => $"{RefreshSessionCachePrefix}{refreshToken}";

    private static string GenerateSecureToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(48))
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}