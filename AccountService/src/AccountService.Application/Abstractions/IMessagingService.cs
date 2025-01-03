using DotNetMessagingRepository.Events.Account;

namespace AccountService.Application.Abstractions;
public interface IMessagingService
{
    Task UserCreated(UserCreated @event);
    Task UserUpdated(UserUpdated @event);
    Task UserDeleted(UserDeleted @event);
    Task SaveEventsToOutbox(CancellationToken ct);
}
