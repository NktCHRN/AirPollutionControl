using AlertService.Domain.Models;
using AlertService.Infrastructure.Abstractions;
using AlertService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlertService.Infrastructure.Seeders;
public sealed class AgglomerationSeeder : IAgglomerationSeeder
{
    private static Agglomeration[] Agglomerations =>
    [
        new Agglomeration {Id = Guid.Parse("0d919f93-581f-4cbf-bfe8-328fdee0039d"), Name = "Kyiv (city with special status)", AdministrationName = "Kyiv City State Administration (department of air quality control)", CountryId=Guid.Parse("51e6a975-9328-46c7-91b8-7ebc2805f7cf")},
        new Agglomeration {Id = Guid.Parse("09c7a22a-dba9-48b9-baf4-e152e7d524ee"), Name = "Zhytomyr Oblast", AdministrationName = "Governor of Zhytomyr Oblast (department of air quality control)", CountryId=Guid.Parse("51e6a975-9328-46c7-91b8-7ebc2805f7cf")},
        new Agglomeration {Id = Guid.Parse("d1336094-f6d6-4bd4-8d01-169b80069a76"), Name = "Capital City of Warsaw", AdministrationName = "Warsaw Department", CountryId = Guid.Parse("5d46c081-480e-4f70-b5f9-91e4dc02a2ea")},
    ];

    public void Seed(ApplicationDbContext dbContext)
    {
        var agglomerations = Agglomerations;

        var existingAgglomerations = dbContext.Agglomerations.Where(c => agglomerations.Select(e => e.Id).Contains(c.Id)).ToList();

        var agglomerationsToAdd = agglomerations.ExceptBy(existingAgglomerations.Select(e => e.Id), c => c.Id);

        dbContext.Agglomerations.AddRange(agglomerationsToAdd);

        dbContext.SaveChanges();
    }

    public async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken ct)
    {
        var agglomerations = Agglomerations;

        var existingAgglomerations = await dbContext.Agglomerations.Where(c => agglomerations.Select(e => e.Id).Contains(c.Id)).ToListAsync(cancellationToken: ct);

        var agglomerationsToAdd = agglomerations.ExceptBy(existingAgglomerations.Select(e => e.Id), c => c.Id);

        await dbContext.Agglomerations.AddRangeAsync(agglomerationsToAdd, ct);

        await dbContext.SaveChangesAsync(ct);
    }
}
