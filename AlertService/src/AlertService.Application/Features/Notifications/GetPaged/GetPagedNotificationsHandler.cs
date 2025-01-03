using AlertService.Domain.Abstractions;
using AlertService.Domain.Models;
using AlertService.Domain.Specifications;
using Application.Abstractions;
using Application.Dto;
using DomainAbstractions.Exceptions;
using MediatR;

namespace AlertService.Application.Features.Notifications.GetPaged;
public sealed class GetPagedNotificationsHandler : IRequestHandler<GetPagedNotificationsQuery, PagedDto<UserNotificationDto>>
{
    private readonly IRepository<UserNotification> userNotificationRepository;
    private readonly ICurrentApplicationUserService currentApplicationUserService;

    public GetPagedNotificationsHandler(IRepository<UserNotification> userNotificationRepository, ICurrentApplicationUserService currentApplicationUserService)
    {
        this.userNotificationRepository = userNotificationRepository;
        this.currentApplicationUserService = currentApplicationUserService;
    }

    public async Task<PagedDto<UserNotificationDto>> Handle(GetPagedNotificationsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentApplicationUserService.Id ?? throw new EntityNotFoundException("User or refresh token was not found");

        var totalCount = await userNotificationRepository.CountAsync(new NotificationsPagedCountSpec(userId), cancellationToken);
        var userNotifications = await userNotificationRepository.ListAsync(new NotificationsPagedSpec(userId, request.PerPage, request.Page), cancellationToken);

        return new PagedDto<UserNotificationDto>(
            userNotifications.Select(c => new UserNotificationDto
            {
                Id = c.NotificationId,
                Scope = c.Notification.Scope,
                Alert = c.Notification.Alert,
                Recommendations = c.Notification.Recommendations,
                OrganizationName = c.Notification.OrganizationName,
                CreatedAt = c.Notification.CreatedAt,
                IsRead = c.IsRead
            })
            .ToList(),
            totalCount);
    }
}
