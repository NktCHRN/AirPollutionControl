using AccountService.Domain.Models;
using Ardalis.Specification;

namespace AccountService.Domain.Specifications;
public sealed class AgglomerationsSearchCountSpec : Specification<Agglomeration>
{
    public AgglomerationsSearchCountSpec(Guid countryId, string? searchText)
    {
        Query.Where(a => a.CountryId == countryId);

        if (!string.IsNullOrEmpty(searchText))
        {
            Query.Where(a => a.Name.ToLower().Contains(searchText.ToLower()));
        }
    }
}
