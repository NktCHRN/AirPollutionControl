using AccountService.Domain.Models;
using Ardalis.Specification;

namespace AccountService.Domain.Specifications;
public sealed class CountriesSearchPagedSpec : Specification<Country>
{
    public CountriesSearchPagedSpec(int perPage, int page, string? searchText)
    {
        if (!string.IsNullOrEmpty(searchText))
        {
            Query.Where(a => a.Name.ToLower().Contains(searchText.ToLower()));
        }

        Query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .AsNoTracking();
    }
}
