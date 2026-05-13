using System.Security.Claims;

namespace SGA_Plataforma.Infrastructure.Utils;

public static class ClaimsPrincipalExtensions
{
    public static int? GetAuthenticatedUserId(this ClaimsPrincipal user)
    {
        var claimValue = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claimValue, out var userId) ? userId : null;
    }
}