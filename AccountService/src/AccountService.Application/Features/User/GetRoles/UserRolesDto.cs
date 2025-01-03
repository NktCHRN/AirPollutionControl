namespace AccountService.Application.Features.User.GetRoles;
public sealed record UserRolesDto(IReadOnlyCollection<string> Roles);
