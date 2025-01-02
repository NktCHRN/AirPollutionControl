using Application.Queries;
using FluentValidation;

namespace Application.Validators;
public class BasePagedQueryValidator : AbstractValidator<PagedQuery>
{
    public BasePagedQueryValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThan(0);

        RuleFor(q => q.PerPage)
            .GreaterThan(0);
    }
}
