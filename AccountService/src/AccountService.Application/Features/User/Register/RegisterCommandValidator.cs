using FluentValidation;

namespace AccountService.Application.Features.User.Register;
public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c)
            .Must(c => !string.IsNullOrEmpty(c.Email) || !string.IsNullOrEmpty(c.PhoneNumber))
            .WithMessage("Either phone number or email must not be empty");

        RuleFor(c => c.Email)
            .EmailAddress();

        RuleFor(c => c.PhoneNumber)
            .Length(5, 15)
            .Must(p => string.IsNullOrEmpty(p) || p.StartsWith("+"))
            .WithMessage("Phone number must start with +.")
            .Must(p => string.IsNullOrEmpty(p) || p[1..].All(c => char.IsDigit(c)))
            .WithMessage("Phone number can contain only + and digits.");

        RuleFor(c => c.FirstName)
            .NotEmpty();

        RuleFor(c => c.LastName)
            .NotEmpty();

        RuleFor(c => c.Birthday)
            .Must(c => c.Year >= 1900 && c.Year <= DateTimeOffset.UtcNow.Year)
            .WithMessage("Birthday must be valid.");
    }
}
