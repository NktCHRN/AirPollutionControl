using AlertService.Domain.Models;
using Ardalis.Specification;

namespace AlertService.Domain.Specifications;
public sealed class NotificationsPagedSpec : Specification<UserNotification>
{
    public NotificationsPagedSpec(Guid userId, int perPage, int page)
    {
        Query
            .Include(un => un.Notification)
            .Where(un => un.UserId == userId)
            .OrderByDescending(un => un.Notification.CreatedAt)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .AsNoTracking();
    }
}
