using Application.Validators;
using FluentValidation;

namespace AccountService.Application.Features.Agglomerations.Search;
public sealed class SearchAgglomerationsQueryValidator : AbstractValidator<SearchAgglomerationsQuery>
{
    public SearchAgglomerationsQueryValidator()
    {
        Include(new BasePagedSearchQueryValidator());
    }
}
