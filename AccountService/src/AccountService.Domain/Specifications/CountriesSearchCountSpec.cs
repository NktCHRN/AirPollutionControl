using AccountService.Domain.Models;
using Ardalis.Specification;

namespace AccountService.Domain.Specifications;
public sealed class CountriesSearchCountSpec : Specification<Country>
{
    public CountriesSearchCountSpec(string? searchText)
    {
        if (!string.IsNullOrEmpty(searchText))
        {
            Query.Where(a => a.Name.ToLower().Contains(searchText.ToLower()));
        }
    }
}
