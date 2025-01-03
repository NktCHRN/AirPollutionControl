using AlertService.Domain.Models;
using Ardalis.Specification;

namespace AlertService.Domain.Specifications;
public sealed class CountryUsersSpec : Specification<User, Guid>
{
    public CountryUsersSpec(Guid countryId)
    {
        Query
            .Where(a => a.AgglomerationId.HasValue && a.Agglomeration!.CountryId == countryId);

        Query.Select(q => q.Id);
    }
}
