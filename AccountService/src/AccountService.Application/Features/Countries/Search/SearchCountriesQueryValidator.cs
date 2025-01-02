using Application.Validators;
using FluentValidation;

namespace AccountService.Application.Features.Countries.Search;
public sealed class SearchCountriesQueryValidator : AbstractValidator<SearchCountriesQuery>
{
    public SearchCountriesQueryValidator()
    {
        Include(new BasePagedSearchQueryValidator());
    }
}
