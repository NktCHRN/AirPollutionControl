using AlertService.Domain.Models;
using Ardalis.Specification;

namespace AlertService.Domain.Specifications;
public sealed class AgglomerationUsersSpec : Specification<User, Guid>
{
    public AgglomerationUsersSpec(Guid agglomerationId)
    {
        Query
            .Where(a => a.AgglomerationId == agglomerationId);

        Query.Select(q => q.Id);
    }
}
