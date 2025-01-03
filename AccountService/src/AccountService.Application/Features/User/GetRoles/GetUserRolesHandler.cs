using Application.Abstractions;
using MediatR;

namespace AccountService.Application.Features.User.GetRoles;
public sealed class GetUserRolesHandler : IRequestHandler<GetUserRolesQuery, UserRolesDto>
{
    private readonly ICurrentApplicationUserService currentApplicationUserService;

    public GetUserRolesHandler(ICurrentApplicationUserService currentApplicationUserService)
    {
        this.currentApplicationUserService = currentApplicationUserService;
    }

    public Task<UserRolesDto> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = currentApplicationUserService.GetRoles();

        return Task.FromResult(new UserRolesDto(roles));
    }
}
