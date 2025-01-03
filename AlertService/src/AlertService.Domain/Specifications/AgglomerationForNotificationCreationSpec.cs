using AlertService.Domain.Models;
using Ardalis.Specification;

namespace AlertService.Domain.Specifications;
public sealed class AgglomerationForNotificationCreationSpec : SingleResultSpecification<Agglomeration>
{
    public AgglomerationForNotificationCreationSpec(Guid agglomerationId)
    {
        Query.Where(a => a.Id == agglomerationId);
    }
}
