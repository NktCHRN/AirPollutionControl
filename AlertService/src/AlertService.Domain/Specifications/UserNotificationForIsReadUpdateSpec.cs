using AlertService.Domain.Models;
using Ardalis.Specification;

namespace AlertService.Domain.Specifications;
public sealed class UserNotificationForIsReadUpdateSpec : SingleResultSpecification<UserNotification>
{
    public UserNotificationForIsReadUpdateSpec(Guid userId, Guid notificationId)
    {
        Query.Where(un => un.UserId == userId && un.NotificationId == notificationId);
    }
}
