using AccountService.Infrastructure.Persistence;

namespace AccountService.Infrastructure.Abstractions;
internal interface IAgglomerationSeeder
{
    Task SeedAsync(ApplicationDbContext dbContext, CancellationToken ct);
    void Seed(ApplicationDbContext dbContext);
}
