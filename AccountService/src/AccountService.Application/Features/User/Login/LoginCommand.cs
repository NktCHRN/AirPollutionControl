using Application.Attributes;
using MediatR;

namespace AccountService.Application.Features.User.Login;
[TransactionalCommand]
public sealed record LoginCommand(string Login, string Password) : IRequest<LoginDto>;

