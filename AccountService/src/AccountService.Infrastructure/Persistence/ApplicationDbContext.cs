using AccountService.Domain.Models;
using Database;
using DomainAbstractions;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AccountService.Infrastructure.Persistence;
public sealed class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DbSet<Country> Countries { get; set; }
    public DbSet<Agglomeration> Agglomerations { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

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
