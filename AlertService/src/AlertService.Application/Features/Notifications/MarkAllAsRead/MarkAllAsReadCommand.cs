using Application.Attributes;
using MediatR;

namespace AlertService.Application.Features.Notifications.MarkAllAsRead;
[TransactionalCommand]
public sealed record MarkAllAsReadCommand : IRequest
{
}
