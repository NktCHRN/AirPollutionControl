using AccountService.Domain.Models;
using Ardalis.Specification;

namespace AccountService.Domain.Specifications;
public sealed class AgglomerationsPagedSpec : Specification<Agglomeration>
{
    public AgglomerationsPagedSpec(Guid countryId, int perPage, int page)
    {
        Query
            .Where(a => a.CountryId == countryId)
            .OrderByDescending(c => c.Name)
            .Skip((page - 1) * perPage)
            .Take(perPage);
    }
}
