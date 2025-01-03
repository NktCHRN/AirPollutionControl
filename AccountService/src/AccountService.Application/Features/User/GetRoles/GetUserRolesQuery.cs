using MediatR;

namespace AccountService.Application.Features.User.GetRoles;
public sealed record GetUserRolesQuery : IRequest<UserRolesDto>;
