using AlertService.Domain.Models;
using Ardalis.Specification;

namespace AlertService.Domain.Specifications;
public sealed class UnreadUserNotificationsSpec : Specification<UserNotification>
{
    public UnreadUserNotificationsSpec(Guid userId)
    {
        Query
            .Where(un => un.UserId == userId && !un.IsRead);
    }
}
