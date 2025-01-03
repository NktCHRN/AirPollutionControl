using AlertService.Domain.Models;
using Database;
using DomainAbstractions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AlertService.Infrastructure.Persistence;
public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Agglomeration> Agglomerations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<BaseDomainEvent>();
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(IInfrastructureMarker))!);
        modelBuilder.ApplySoftDeleteFilter();

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
