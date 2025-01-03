using AlertService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlertService.Infrastructure.EntityConfigurations;
public sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.FirstName)
            .HasMaxLength(512);
        builder.Property(e => e.LastName)
            .HasMaxLength(512);

        builder.Navigation(u => u.Agglomeration)
            .AutoInclude();

        builder.HasIndex(e => e.PhoneNumber);
    }
}
