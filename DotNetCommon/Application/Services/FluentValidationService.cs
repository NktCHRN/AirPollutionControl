using FluentValidation.Results;

namespace Application.Services;
public static class FluentValidationService
{
    public static string FluentValidationFailuresToString(IEnumerable<ValidationFailure> failures)
    {
        return string.Join(Environment.NewLine, failures.Select(f => f.ErrorMessage));
    }
}
