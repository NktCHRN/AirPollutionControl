using AlertService.Domain.Models;
using Ardalis.Specification;

namespace AlertService.Domain.Specifications;
public sealed class CountryForNotificationCreationSpec : SingleResultSpecification<Country>
{
    public CountryForNotificationCreationSpec(Guid countryId)
    {
        Query
            .Where(c => c.Id == countryId);
    }
}
