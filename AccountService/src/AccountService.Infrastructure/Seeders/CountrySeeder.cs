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
                Id = Guid.Parse("51e6a975-9328-46c7-91b8-7ebc2805f7cf"),
                Name = "Ukraine",
                AdministrationName = "Ministry of Environmental Protection and Natural Resources (Ukraine)"
            },
            new Country
            {
                Id = Guid.Parse("5d46c081-480e-4f70-b5f9-91e4dc02a2ea"),
                Name = "Poland",
                AdministrationName = "Ministry of Climate and Environment (Poland)"
            }
        ];

    public void Seed(ApplicationDbContext dbContext)
    {
        var expectedCountries = ExpectedCountries;

        var existingCountries = dbContext.Countries.Where(c => expectedCountries.Select(e => e.Id).Contains(c.Id)).ToList();

        var countriesToAdd = expectedCountries.ExceptBy(existingCountries.Select(e => e.Id), c => c.Id);

        dbContext.Countries.AddRange(countriesToAdd);

        dbContext.SaveChanges();
    }

    public async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken ct)
    {
        var expectedCountries = ExpectedCountries;

        var existingCountries = await dbContext.Countries.Where(c => expectedCountries.Select(e => e.Id).Contains(c.Id)).ToListAsync(cancellationToken: ct);

        var countriesToAdd = expectedCountries.ExceptBy(existingCountries.Select(e => e.Id), c => c.Id);

        await dbContext.Countries.AddRangeAsync(countriesToAdd, ct);

        await dbContext.SaveChangesAsync(ct);
    }
}
