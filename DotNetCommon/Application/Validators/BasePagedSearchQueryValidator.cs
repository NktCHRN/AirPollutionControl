using Application.Queries;
using FluentValidation;

namespace Application.Validators;
public class BasePagedSearchQueryValidator : AbstractValidator<PagedSearchQuery>
{
    public BasePagedSearchQueryValidator()
    {
        Include(new BasePagedQueryValidator());
    }
}
