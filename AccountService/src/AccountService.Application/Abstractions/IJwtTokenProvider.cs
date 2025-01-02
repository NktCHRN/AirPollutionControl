using System.Security.Claims;

namespace AccountService.Application.Abstractions;
public interface IJwtTokenProvider
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    public string GenerateRefreshToken();

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
