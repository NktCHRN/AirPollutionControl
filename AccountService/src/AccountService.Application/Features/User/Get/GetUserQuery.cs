using MediatR;

namespace AccountService.Application.Features.User.Get;
public sealed record GetUserQuery : IRequest<UserDto>;
