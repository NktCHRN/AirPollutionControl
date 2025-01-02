using Application.Attributes;
using MediatR;

namespace AccountService.Application.Features.User.RevokeToken;
[TransactionalCommand]
public sealed record RevokeTokenCommand(string RefreshToken) : IRequest;
