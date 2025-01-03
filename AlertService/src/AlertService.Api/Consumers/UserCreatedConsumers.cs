using AlertService.Application.Features.Users.SaveUser;
using DotNetMessagingRepository.Events.Account;
using MassTransit;
using MediatR;

namespace AlertService.Api.Consumers;

public sealed class UserCreatedConsumers : IConsumer<UserCreated>
{
    private readonly IMediator mediator;
    private readonly ILogger<UserCreatedConsumers> logger;

    public UserCreatedConsumers(IMediator mediator, ILogger<UserCreatedConsumers> logger)
    {
        this.mediator = mediator;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        logger.LogInformation("UserCreated was received by AlertService {event}", context.Message);

        var command = new SaveUserCommand
        {
            Id = context.Message.Id,
            AgglomerationId = context.Message.AgglomerationId,
            Email = context.Message.Email,
            PhoneNumber = context.Message.PhoneNumber,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName
        };
        await mediator.Send(command);
    }
}
