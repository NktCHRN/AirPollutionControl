using AlertService.Domain.Abstractions;
using AlertService.Domain.Models;
using AlertService.Domain.Specifications;
using Application.Abstractions;
using DomainAbstractions.Exceptions;
using MediatR;

namespace AlertService.Application.Features.Notifications.MarkAllAsRead;
public sealed class MarkAllAsReadHandler : IRequestHandler<MarkAllAsReadCommand>
{
    private readonly IRepository<UserNotification> userNotificationRepository;
    private readonly ICurrentApplicationUserService currentApplicationUserService;

    public MarkAllAsReadHandler(IRepository<UserNotification> userNotificationRepository, ICurrentApplicationUserService currentApplicationUserService)
    {
        this.userNotificationRepository = userNotificationRepository;
        this.currentApplicationUserService = currentApplicationUserService;
    }

    public async Task Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        var userId = currentApplicationUserService.Id ?? throw new EntityNotFoundException("User or refresh token was not found");

        var unreadNotifications = await userNotificationRepository.ListAsync(new UnreadUserNotificationsSpec(userId), cancellationToken);

        unreadNotifications.ForEach(notification => notification.IsRead = true);
        await userNotificationRepository.UpdateRangeAsync(unreadNotifications, cancellationToken);
    }
}
