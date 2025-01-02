using AccountService.Application.Abstractions;
using AccountService.Application.Options;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Models;
using DomainAbstractions.Exceptions;
using DomainAbstractions.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace AccountService.Application.Features.User.Login;
public sealed class LoginHandler : IRequestHandler<LoginCommand, LoginDto>
{
    private readonly UserManager<Domain.Models.User> userManager;
    private readonly IJwtTokenProvider jwtTokenProvider;
    private readonly IRepository<RefreshToken> refreshTokenRepository;
    private readonly TokenProvidersOptions tokenProvidersOptions;
    private readonly TimeProvider timeProvider;

    public LoginHandler(UserManager<Domain.Models.User> userManager, IJwtTokenProvider jwtTokenProvider, IRepository<RefreshToken> refreshTokenRepository, IOptions<TokenProvidersOptions> tokenProvidersOptions, TimeProvider timeProvider)
    {
        this.userManager = userManager;
        this.jwtTokenProvider = jwtTokenProvider;
        this.refreshTokenRepository = refreshTokenRepository;
        this.tokenProvidersOptions = tokenProvidersOptions.Value;
        this.timeProvider = timeProvider;
    }

    public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.User? user;

        if (request.Login.Contains('@'))
        {
            user = await userManager.FindByEmailAsync(request.Login);
        }
        else
        {
            user = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.Login, cancellationToken: cancellationToken);
        }

        var checkPasswordResult = user is not null && await userManager.CheckPasswordAsync(user, request.Password);
        if (!checkPasswordResult)
        {
            throw new UserUnauthorizedException("Either login or password is incorrect.");
        }

        var roles = await userManager.GetRolesAsync(user!);

        var refreshToken = jwtTokenProvider.GenerateRefreshToken();
        await refreshTokenRepository.AddAsync(new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiryTime = timeProvider.GetUtcNow().AddDays(tokenProvidersOptions.RefreshTokenLifetimeInDays)
        }, cancellationToken);

        return new LoginDto(jwtTokenProvider.GenerateAccessToken(GetClaims(user, roles)), refreshToken);
    }

    private static List<Claim> GetClaims(Domain.Models.User user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Email, user.Email ?? string.Empty),
            new (ClaimTypes.GivenName, UserAccountService.GetFullName(user.FirstName, user.MiddleName, user.LastName, user.Patronymic)),
            new (ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
            new ("AgglomerationId", user.AgglomerationId?.ToString() ?? string.Empty),
            new ("AgglomerationName", user.Agglomeration?.Name ?? string.Empty),
            new ("CountryId", user.Agglomeration?.CountryId.ToString() ?? string.Empty),
            new (ClaimTypes.Country, user.Agglomeration?.Country?.Name ?? string.Empty),
        };
        foreach (var role in roles)
        {
            claims.Add(new (ClaimTypes.Role, role));
        }

        return claims;
    }
}
