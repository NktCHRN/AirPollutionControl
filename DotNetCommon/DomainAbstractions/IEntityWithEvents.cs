namespace DomainAbstractions;
public interface IEntityWithEvents
{
    IReadOnlyCollection<BaseDomainEvent> DomainEvents { get; }
    void AddDomainEvent(BaseDomainEvent domainEvent);
    void RemoveDomainEvent(BaseDomainEvent domainEvent);
    void ClearDomainEvents();
}
