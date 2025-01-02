using AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.EntityConfigurations;
public sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.FirstName)
            .HasMaxLength(512);
        builder.Property(e => e.MiddleName)
            .HasMaxLength(512);
        builder.Property(e => e.LastName)
            .HasMaxLength(512);
        builder.Property(e => e.Patronymic)
            .HasMaxLength(512);

        builder.Property(e => e.PositionName)
            .HasMaxLength(256);

        builder.Property(e => e.RestrictionNote)
            .HasMaxLength(1024);

        builder.Navigation(u => u.Agglomeration)
            .AutoInclude();
    }
}
