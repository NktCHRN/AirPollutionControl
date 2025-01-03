using AlertService.Domain.Abstractions;
using AlertService.Domain.Models;
using AlertService.Domain.Specifications;
using Application.Abstractions;
using DomainAbstractions.Exceptions;
using MediatR;

namespace AlertService.Application.Features.Notifications.UpdateIsRead;
public sealed class UpdateIsReadHandler : IRequestHandler<UpdateIsReadCommand>
{
    private readonly IRepository<UserNotification> userNotificationRepository;
    private readonly ICurrentApplicationUserService currentApplicationUserService;

    public UpdateIsReadHandler(IRepository<UserNotification> userNotificationRepository, ICurrentApplicationUserService currentApplicationUserService)
    {
        this.userNotificationRepository = userNotificationRepository;
        this.currentApplicationUserService = currentApplicationUserService;
    }

    public async Task Handle(UpdateIsReadCommand request, CancellationToken cancellationToken)
    {
        var userId = currentApplicationUserService.Id ?? throw new EntityNotFoundException("User or refresh token was not found");

        var userNotification = await userNotificationRepository.FirstOrDefaultAsync(new UserNotificationForIsReadUpdateSpec(userId, request.NotificationId), cancellationToken)
            ?? throw new EntityNotFoundException("Notification was not found");

        userNotification.IsRead = request.IsRead;
        await userNotificationRepository.UpdateAsync(userNotification, cancellationToken);
    }
}
