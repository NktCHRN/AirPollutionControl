using AccountService.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MessagingRoles = DotNetMessagingRepository.Common.Roles;

namespace AccountService.Infrastructure.Seeders;
internal sealed class RoleSeeder : IRoleSeeder
{
    private HashSet<string> Roles =>
    [
        MessagingRoles.User,
        MessagingRoles.RestrictedUser,
        MessagingRoles.ConfirmedUser,
        MessagingRoles.AirQualityControlService,
        MessagingRoles.Commission,
        MessagingRoles.AgglomerationGovernmentMember,
        MessagingRoles.AgglomerationAdmin,
        MessagingRoles.CountryGovernmentMember,
        MessagingRoles.CountryAdmin,
        MessagingRoles.GlobalAdmin
    ];

    public void Seed(RoleManager<IdentityRole<Guid>> roleManager)
    {
        var roles = Roles;
        var currentRoles = roleManager.Roles.ToList();

        roles.ExceptWith(currentRoles.Select(r => r.Name!));
        foreach (var role in roles)
        {
            roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = role
            }).Wait();
        }
    }

    public async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager, CancellationToken ct)
    {
        var roles = Roles;
        var currentRoles = await roleManager.Roles.ToListAsync(ct);

        roles.ExceptWith(currentRoles.Select(r => r.Name!));
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = role
            });
        }
    }
}
