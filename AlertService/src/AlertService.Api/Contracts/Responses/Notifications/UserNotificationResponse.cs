using AlertService.Api.Contracts.Enums;

namespace AlertService.Api.Contracts.Responses.Notifications;

public sealed record UserNotificationResponse
{
    public required Guid Id { get; set; }
    public required string? Alert { get; set; }
    public required string? Recommendations { get; set; }
    public required NotificationScope Scope { get; set; }
    public required string OrganizationName { get; set; } = string.Empty;
    public required DateTimeOffset CreatedAt { get; set; }

    public required bool IsRead { get; set; }
}
