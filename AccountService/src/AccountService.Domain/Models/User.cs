using DomainAbstractions;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Domain.Models;
public class User : IdentityUser<Guid>, ISoftDeletable, IEntityWithEvents
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Patronymic {  get; set; }
    public DateOnly Birthday { get; set; }
    public string? RestrictionNote { get; set; }
    public DateTimeOffset? RestrictionEnd { get; set; }
    public string? PositionName { get; set; }
    public Guid? AgglomerationId { get; set; }                      // Can be null for global admins.
    public Agglomeration? Agglomeration { get; set; } = null!;
    public IList<RefreshToken> RefreshTokens { get; set; } = [];
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsDeleted { get; set; }

    private readonly List<BaseDomainEvent> domainEvents = [];
    public IReadOnlyCollection<BaseDomainEvent> DomainEvents => domainEvents;
    public void AddDomainEvent(BaseDomainEvent domainEvent)
    {
        domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        domainEvents.Clear();
    }
    public void RemoveDomainEvent(BaseDomainEvent domainEvent)
    {
        domainEvents.Remove(domainEvent);
    }
}
