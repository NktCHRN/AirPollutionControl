using AlertService.Domain.Models;
using Ardalis.Specification;

namespace AlertService.Domain.Specifications;
public sealed class NotificationsPagedCountSpec : Specification<UserNotification>
{
    public NotificationsPagedCountSpec(Guid userId)
    {
        Query.
            Where(n => n.UserId == userId);
    }
}
