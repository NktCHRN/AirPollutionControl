using AccountService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Infrastructure.Abstractions;
internal interface IRoleSeeder
{
    Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager, CancellationToken ct);
    void Seed(RoleManager<IdentityRole<Guid>> roleManager);
}
