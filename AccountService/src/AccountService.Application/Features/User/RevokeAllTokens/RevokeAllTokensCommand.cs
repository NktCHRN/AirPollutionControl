using MediatR;

namespace AccountService.Application.Features.User.RevokeAllTokens;
public sealed record RevokeAllTokensCommand() : IRequest;

