namespace AlertService.Api.Contracts.Requests.NotificationsAdmin;

public sealed record CreateNotificationRequest
{
    public string? Alert { get; set; }
    public string? Recommendations { get; set; }
}
