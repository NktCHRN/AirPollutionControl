using AccountService.Domain.Models;
using Ardalis.Specification;

namespace AccountService.Domain.Specifications;
public sealed class CountriesPagedSpec : Specification<Country>
{
    public CountriesPagedSpec(int perPage, int page)
    {
        Query
            .OrderByDescending(c => c.Name)
            .Skip((page - 1) * perPage)
            .Take(perPage);
    }
}
