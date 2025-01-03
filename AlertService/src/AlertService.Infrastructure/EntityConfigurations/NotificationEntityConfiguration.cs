using AlertService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlertService.Infrastructure.EntityConfigurations;
public sealed class NotificationEntityConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(a => a.Alert)
            .HasMaxLength(4000);
        builder.Property(a => a.Recommendations)
            .HasMaxLength(4000);

        builder.Property(a => a.OrganizationName)
            .HasMaxLength(512);
    }
}
