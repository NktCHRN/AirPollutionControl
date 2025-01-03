using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using DomainAbstractions;

namespace Database.Interceptors;
public sealed class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    public DispatchDomainEventsInterceptor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var newResult = base.SavingChanges(eventData, result);

        DispatchDomainEventsAsync(eventData.Context).GetAwaiter().GetResult();

        return newResult;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var newResult = await base.SavingChangesAsync(eventData, result, cancellationToken);

        await DispatchDomainEventsAsync(eventData.Context);

        return newResult;
    }

    public async Task DispatchDomainEventsAsync(DbContext? dbContext)
    {
        if (dbContext is null)
        {
            return;
        }

        var domainEvents = new List<BaseDomainEvent>();
        foreach (var entity in dbContext.ChangeTracker.Entries<IEntityWithEvents>().Select(e => e.Entity))
        {
            domainEvents.AddRange(entity.DomainEvents);
            entity.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }
}
