using AlertService.Domain.Enums;
using DomainAbstractions;

namespace AlertService.Domain.Models;
public class Notification : ISoftDeletable, IEntityWithEvents
{
    public Guid Id { get; set; }
    public string? Alert { get; set; }
    public string? Recommendations { get; set; }
    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;
    public NotificationScope Scope { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

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
