using AlertService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlertService.Infrastructure.EntityConfigurations;
public sealed class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.Property(a => a.Name)
            .HasMaxLength(256);
        builder.Property(a => a.AdministrationName)
            .HasMaxLength(512);
    }
}
