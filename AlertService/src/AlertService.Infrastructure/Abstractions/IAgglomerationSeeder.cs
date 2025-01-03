using AlertService.Infrastructure.Persistence;

namespace AlertService.Infrastructure.Abstractions;
internal interface IAgglomerationSeeder
{
    Task SeedAsync(ApplicationDbContext dbContext, CancellationToken ct);
    void Seed(ApplicationDbContext dbContext);
}
