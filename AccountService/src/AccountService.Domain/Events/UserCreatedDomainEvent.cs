using DomainAbstractions;

namespace AccountService.Domain.Events;
public sealed class UserCreatedDomainEvent : BaseDomainEvent
{
    public required Guid UserId { get; set; }
}
