using AccountService.Application.Abstractions;
using AccountService.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.User.UserCreated;
public sealed class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly UserManager<Domain.Models.User> userManager;
    private readonly IMessagingService messagingService;

    public UserCreatedDomainEventHandler(UserManager<Domain.Models.User> userManager, IMessagingService messagingService)
    {
        this.userManager = userManager;
        this.messagingService = messagingService;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = userManager.Users.AsNoTracking().FirstOrDefault(u => u.Id == notification.UserId)
            ?? throw new InvalidOperationException("Currently created user was not found");

        await messagingService.UserCreated(new DotNetMessagingRepository.Events.Account.UserCreated
        {
            Id = user.Id,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
            Birthday = user.Birthday,
            AgglomerationId = user.AgglomerationId,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            RestrictionEnd = user.RestrictionEnd,
            RestrictionNote = user.RestrictionNote,
            PositionName = user.PositionName
        });
        await messagingService.SaveEventsToOutbox(cancellationToken);
    }
}
