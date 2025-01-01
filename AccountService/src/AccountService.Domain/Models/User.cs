using DomainAbstractions;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Domain.Models;
public sealed class User : IdentityUser<Guid>, ISoftDeletable
{
    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly Birthday { get; set; }
    public Guid AgglomerationId { get; set; }
    public Agglomeration Agglomeration { get; set; } = null!;
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = null!;
    public IList<RefreshToken> RefreshTokens { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
