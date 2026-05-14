using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SGA_Plataforma.Api.Utils;

public static class JwtConfiguration
{
    private const int MinimumJwtKeySizeInBits = 256;
    private const int MinimumJwtKeySizeInBytes = MinimumJwtKeySizeInBits / 8;

    public static string GetKey(IConfiguration configuration)
    {
        var key = GetConfigurationValue(configuration, "Jwt:Key", "JWT_KEY");

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(
                "JWT Key nao configurado. Defina 'Jwt:Key' no appsettings ou a variavel de ambiente JWT_KEY.");
        }

        var keySizeInBytes = Encoding.UTF8.GetByteCount(key);
        if (keySizeInBytes < MinimumJwtKeySizeInBytes)
        {
            throw new InvalidOperationException(
                $"JWT Key invalida. O HS256 exige no minimo {MinimumJwtKeySizeInBits} bits ({MinimumJwtKeySizeInBytes} bytes), mas a chave configurada possui {keySizeInBytes} bytes.");
        }

        return key;
    }

    public static SymmetricSecurityKey GetSigningKey(IConfiguration configuration)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetKey(configuration)));
    }

    public static string GetIssuer(IConfiguration configuration)
    {
        return GetRequiredConfigurationValue(
            configuration,
            "Jwt:Issuer",
            "JWT_ISSUER",
            "JWT Issuer nao configurado. Defina 'Jwt:Issuer' no appsettings ou a variavel de ambiente JWT_ISSUER.");
    }

    public static string GetAudience(IConfiguration configuration)
    {
        return GetRequiredConfigurationValue(
            configuration,
            "Jwt:Audience",
            "JWT_AUDIENCE",
            "JWT Audience nao configurado. Defina 'Jwt:Audience' no appsettings ou a variavel de ambiente JWT_AUDIENCE.");
    }

    public static double GetExpireMinutes(IConfiguration configuration)
    {
        var rawValue = GetConfigurationValue(configuration, "Jwt:ExpireMinutes", "JWT_EXPIRE_MINUTES");
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            throw new InvalidOperationException(
                "JWT ExpireMinutes nao configurado. Defina 'Jwt:ExpireMinutes' no appsettings ou a variavel de ambiente JWT_EXPIRE_MINUTES.");
        }

        if (!double.TryParse(rawValue, out var expireMinutes) || expireMinutes <= 0)
        {
            throw new InvalidOperationException(
                "JWT ExpireMinutes invalido. Informe um numero maior que zero em 'Jwt:ExpireMinutes' ou JWT_EXPIRE_MINUTES.");
        }

        return expireMinutes;
    }

    private static string GetRequiredConfigurationValue(
        IConfiguration configuration,
        string configKey,
        string environmentKey,
        string errorMessage)
    {
        var value = GetConfigurationValue(configuration, configKey, environmentKey);
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException(errorMessage);
        }

        return value;
    }

    private static string? GetConfigurationValue(IConfiguration configuration, string configKey, string environmentKey)
    {
        var configuredValue = configuration[configKey];
        if (!string.IsNullOrWhiteSpace(configuredValue))
        {
            return configuredValue;
        }

        return Environment.GetEnvironmentVariable(environmentKey);
    }
}