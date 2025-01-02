using AccountService.Domain.Models;
using AccountService.Infrastructure.Abstractions;
using AccountService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Seeders;
public sealed class AgglomerationSeeder : IAgglomerationSeeder
{
    private (Agglomeration agglomeration, string countryName)[] Agglomerations => new (Agglomeration agglomeration, string countryName)[]
    {
        new (new Agglomeration {Name = "Kyiv (city with special status)", AdministrationName = "Kyiv City State Administration (department of air quality control)"}, "Ukraine"),
                new (new Agglomeration {Name = "Zhytomyr Oblast", AdministrationName = "Governor of Zhytomyr Oblast (department of air quality control)"}, "Ukraine"),
                        new (new Agglomeration {Name = "Capital City of Warsaw", AdministrationName = "Warsaw Department"}, "Poland"),
    };

    public void Seed(ApplicationDbContext dbContext)
    {
        var agglomerations = Agglomerations;
        var countries = dbContext.Countries.Include(c => c.Agglomerations).Where(c => agglomerations.Select(a => a.countryName).Contains(c.Name)).ToList();

        foreach (var (agglomeration, countryName) in agglomerations)
        {
            var country = countries.First(c => string.Equals(c.Name, countryName, StringComparison.InvariantCultureIgnoreCase));
            if (country.Agglomerations.Any(a => string.Equals(a.Name, agglomeration.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                continue;
            }

            agglomeration.Country = country;
            dbContext.Add(agglomeration);
        }

        dbContext.SaveChanges();
    }

    public async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken ct)
    {
        var agglomerations = Agglomerations;
        var countries = await dbContext.Countries.Include(c => c.Agglomerations).Where(c => agglomerations.Select(a => a.countryName).Contains(c.Name)).ToListAsync(cancellationToken: ct);

        foreach (var (agglomeration, countryName) in agglomerations)
        {
            var country = countries.First(c => string.Equals(c.Name, countryName, StringComparison.InvariantCultureIgnoreCase));
            if (country.Agglomerations.Any(a => string.Equals(a.Name, agglomeration.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                continue;
            }

            agglomeration.Country = country;
            await dbContext.AddAsync(agglomeration, ct);
        }

        await dbContext.SaveChangesAsync(ct);
    }
}
