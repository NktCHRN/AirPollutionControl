using AlertService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlertService.Infrastructure.EntityConfigurations;
public sealed class AgglomerationEntityConfiguration : IEntityTypeConfiguration<Agglomeration>
{
    public void Configure(EntityTypeBuilder<Agglomeration> builder)
    {
        builder.Property(a => a.Name)
            .HasMaxLength(512);
        builder.Property(a => a.AdministrationName)
            .HasMaxLength(512);

        builder.Navigation(u => u.Country)
            .AutoInclude();
    }
}
