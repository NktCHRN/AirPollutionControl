using DomainAbstractions;

namespace AccountService.Domain.Models;
public class Country : IEntityWithEvents, ISoftDeletable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AdministrationName { get; set; } = string.Empty;
    public IList<Agglomeration> Agglomerations { get; set; } = [];
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
