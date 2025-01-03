using Application.Attributes;
using MediatR;

namespace AlertService.Application.Features.Notifications.UpdateIsRead;
[TransactionalCommand]
public sealed record UpdateIsReadCommand : IRequest
{
    public Guid NotificationId { get; set; }
    public bool IsRead { get; set; }
}
