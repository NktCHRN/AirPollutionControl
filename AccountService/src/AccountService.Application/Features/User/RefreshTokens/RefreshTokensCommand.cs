using Application.Attributes;
using MediatR;

namespace AccountService.Application.Features.User.RefreshTokens;
[TransactionalCommand]
public sealed record RefreshTokensCommand(string AccessToken, string RefreshToken) : IRequest<TokensDto>;
