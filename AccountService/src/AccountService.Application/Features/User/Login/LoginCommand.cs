using MediatR;

namespace AccountService.Application.Features.User.Login;
public sealed record LoginCommand : IRequest<LoginDto>
{
}
