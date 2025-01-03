using AlertService.Domain.Models;
using Ardalis.Specification;

namespace AlertService.Domain.Specifications;
public sealed class UserByIdSpec : SingleResultSpecification<User>
{
    public UserByIdSpec(Guid userId)
    {
        Query
            .Where(u => u.Id == userId);
    }
}
