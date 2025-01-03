using AccountService.Application.Abstractions;
using AccountService.Infrastructure.Persistence;
using DotNetMessagingRepository.Events.Account;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AccountService.Infrastructure.Messaging;
public sealed class MessagingService : IMessagingService
{
    private readonly IPublishEndpoint publishEndpoint;
    private readonly ILogger<MessagingService> logger;
    private readonly ApplicationDbContext applicationDbContext;

    public MessagingService(IPublishEndpoint publishEndpoint, ILogger<MessagingService> logger, ApplicationDbContext applicationDbContext)
    {
        this.publishEndpoint = publishEndpoint;
        this.logger = logger;
        this.applicationDbContext = applicationDbContext;
    }

    public async Task UserCreated(UserCreated @event)
    {
        logger.LogInformation("Publishing UserCreated event {event}", @event);
        await publishEndpoint.Publish(@event);
    }

    public async Task UserDeleted(UserDeleted @event)
    {
        logger.LogInformation("Publishing UserDeleted event {event}", @event);
        await publishEndpoint.Publish(@event);
    }

    public async Task UserUpdated(UserUpdated @event)
    {
        logger.LogInformation("Publishing UserUpdated event {event}", @event);
        await publishEndpoint.Publish(@event);
    }

    public async Task SaveEventsToOutbox(CancellationToken ct)
    {
        await applicationDbContext.SaveChangesAsync(ct);
    }
}
