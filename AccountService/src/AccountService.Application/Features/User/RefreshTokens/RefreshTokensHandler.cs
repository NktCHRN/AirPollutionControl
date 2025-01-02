using AccountService.Application.Abstractions;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Models;
using AccountService.Domain.Specifications;
using DomainAbstractions.Exceptions;
using MediatR;
using System.Security;
using System.Security.Claims;

namespace AccountService.Application.Features.User.RefreshTokens;
public sealed class RefreshTokensHandler : IRequestHandler<RefreshTokensCommand, TokensDto>
{
    private readonly IJwtTokenProvider jwtTokenProvider;
    private readonly IRepository<RefreshToken> refreshTokenRepository;
    private readonly TimeProvider timeProvider;

    public RefreshTokensHandler(IJwtTokenProvider jwtTokenProvider, IRepository<RefreshToken> refreshTokenRepository, TimeProvider timeProvider)
    {
        this.jwtTokenProvider = jwtTokenProvider;
        this.refreshTokenRepository = refreshTokenRepository;
        this.timeProvider = timeProvider;
    }

    public async Task<TokensDto> Handle(RefreshTokensCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var principal = jwtTokenProvider.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = GetId(principal) ?? throw new EntityNotFoundException("No info about user id in the token");
            var refreshTokenModel = await refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenByUserIdAndTokenSpec(userId, request.RefreshToken), cancellationToken);
            if (refreshTokenModel is null
                || refreshTokenModel.ExpiryTime <= timeProvider.GetUtcNow())
            {
                throw new UserUnauthorizedException("Refresh token is expired.");
            }

            var newAccessToken = jwtTokenProvider.GenerateAccessToken(principal.Claims);
            var newRefreshToken = jwtTokenProvider.GenerateRefreshToken();
            refreshTokenModel.Token = newRefreshToken;
            await refreshTokenRepository.UpdateAsync(refreshTokenModel, cancellationToken);

            return new TokensDto(newAccessToken, newRefreshToken);
        }
        catch (SecurityException ex)
        {
            throw new UserUnauthorizedException(ex.Message, ex);
        }
    }

    private static Guid? GetId(ClaimsPrincipal? user)
    {
        if (Guid.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid parsed))
            return parsed;
        return null;
    }
}
