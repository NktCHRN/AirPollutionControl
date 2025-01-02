using AccountService.Domain.Models;
using AccountService.Infrastructure.Abstractions;
using AccountService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Seeders;
public sealed class CountrySeeder : ICountrySeeder
{
    private Country[] ExpectedCountries =>
        [
            new Country
            {
                Name = "Ukraine",
                AdministrationName = "Ministry of Environmental Protection and Natural Resources (Ukraine)"
            },
            new Country
            {
                Name = "Poland",
                AdministrationName = "Ministry of Climate and Environment (Poland)"
            }
        ];

    public void Seed(ApplicationDbContext dbContext)
    {
        var expectedCountries = ExpectedCountries;

        var existingCountries = dbContext.Countries.Where(c => expectedCountries.Select(e => e.Name).Contains(c.Name)).ToList();

        var countriesToAdd = expectedCountries.ExceptBy(existingCountries.Select(e => e.Name), c => c.Name);

        dbContext.Countries.AddRange(countriesToAdd);

        dbContext.SaveChanges();
    }

    public async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken ct)
    {
        var expectedCountries = ExpectedCountries;

        var existingCountries = await dbContext.Countries.Where(c => expectedCountries.Select(e => e.Name).Contains(c.Name)).ToListAsync(cancellationToken: ct);

        var countriesToAdd = expectedCountries.ExceptBy(existingCountries.Select(e => e.Name), c => c.Name);

        await dbContext.Countries.AddRangeAsync(countriesToAdd, ct);

        await dbContext.SaveChangesAsync(ct);
    }
}
