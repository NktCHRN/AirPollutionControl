using AccountService.Domain.Abstractions;
using AccountService.Domain.Models;
using AccountService.Domain.Specifications;
using Application.Abstractions;
using DomainAbstractions.Exceptions;
using MediatR;

namespace AccountService.Application.Features.User.RevokeAllTokens;
public sealed class RevokeAllTokensHandler : IRequestHandler<RevokeAllTokensCommand>
{
    private readonly IRepository<RefreshToken> refreshTokenRepository;
    private readonly ICurrentApplicationUserService currentApplicationUserService;
    private readonly TimeProvider timeProvider;

    public RevokeAllTokensHandler(IRepository<RefreshToken> refreshTokenRepository, ICurrentApplicationUserService currentApplicationUserService, TimeProvider timeProvider)
    {
        this.refreshTokenRepository = refreshTokenRepository;
        this.currentApplicationUserService = currentApplicationUserService;
        this.timeProvider = timeProvider;
    }

    public async Task Handle(RevokeAllTokensCommand request, CancellationToken cancellationToken)
    {
        var userId = currentApplicationUserService.Id ?? throw new EntityNotFoundException("User or refresh token was not found");

        var currentTime = timeProvider.GetUtcNow();
        var refreshTokensModels = await refreshTokenRepository.ListAsync(new ActiveRefreshTokens(userId, currentTime), cancellationToken);

        await refreshTokenRepository.DeleteRangeAsync(refreshTokensModels, cancellationToken);
    }
}
