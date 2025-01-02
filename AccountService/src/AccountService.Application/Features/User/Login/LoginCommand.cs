using Application.Attributes;
using MediatR;

namespace AccountService.Application.Features.User.Login;
[TransactionalCommand]
public sealed record LoginCommand : IRequest<LoginDto>
{
}
