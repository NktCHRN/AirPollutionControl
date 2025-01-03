using DomainAbstractions;

namespace AlertService.Domain.Models;
public class UserNotification : ISoftDeletable
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid NotificationId { get; set; }
    public Notification Notification { get; set; } = null!;
    public bool IsRead { get; set; }
    public bool IsDeleted { get; set; }
}
