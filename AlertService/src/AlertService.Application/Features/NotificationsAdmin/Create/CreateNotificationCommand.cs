using MediatR;

namespace AlertService.Application.Features.NotificationsAdmin.Create;
public sealed record CreateNotificationCommand : IRequest
{
    public string? Alert { get; set; }
    public string? Recommendations { get; set; }
}
