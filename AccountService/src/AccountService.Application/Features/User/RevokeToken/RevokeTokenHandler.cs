using AccountService.Domain.Abstractions;
using AccountService.Domain.Models;
using AccountService.Domain.Specifications;
using Application.Abstractions;
using DomainAbstractions.Exceptions;
using MediatR;

namespace AccountService.Application.Features.User.RevokeToken;
public sealed class RevokeTokenHandler : IRequestHandler<RevokeTokenCommand>
{
    private readonly IRepository<RefreshToken> refreshTokenRepository;
    private readonly ICurrentApplicationUserService currentApplicationUserService;

    public RevokeTokenHandler(IRepository<RefreshToken> refreshTokenRepository, ICurrentApplicationUserService currentApplicationUserService)
    {
        this.refreshTokenRepository = refreshTokenRepository;
        this.currentApplicationUserService = currentApplicationUserService;
    }

    public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = currentApplicationUserService.Id ?? throw new EntityNotFoundException("User or refresh token was not found");

        var refreshTokenModel = await refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenByUserIdAndTokenSpec(userId, request.RefreshToken), cancellationToken)
            ?? throw new EntityNotFoundException("User or refresh token was not found");

        await refreshTokenRepository.DeleteAsync(refreshTokenModel, cancellationToken);
    }
}
