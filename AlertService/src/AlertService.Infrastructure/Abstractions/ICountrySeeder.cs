using AlertService.Infrastructure.Persistence;

namespace AlertService.Infrastructure.Abstractions;
internal interface ICountrySeeder
{
    Task SeedAsync(ApplicationDbContext dbContext, CancellationToken ct);
    void Seed(ApplicationDbContext dbContext);
}
