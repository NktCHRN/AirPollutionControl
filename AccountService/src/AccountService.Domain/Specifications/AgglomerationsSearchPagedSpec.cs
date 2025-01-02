using AccountService.Domain.Models;
using Ardalis.Specification;

namespace AccountService.Domain.Specifications;
public sealed class AgglomerationsSearchPagedSpec : Specification<Agglomeration>
{
    public AgglomerationsSearchPagedSpec(Guid countryId, int perPage, int page, string? searchText)
    {
        Query
            .Where(a => a.CountryId == countryId);

        if (!string.IsNullOrEmpty(searchText))
        {
            Query.Where(a => a.Name.ToLower().Contains(searchText.ToLower()));
        }

        Query
            .OrderByDescending(c => c.Name)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .AsNoTracking();
    }
}
