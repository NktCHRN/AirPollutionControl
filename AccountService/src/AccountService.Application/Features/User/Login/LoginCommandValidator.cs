using FluentValidation;

namespace AccountService.Application.Features.User.Login;
public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(l => l.Login)
            .NotEmpty();

        RuleFor(l => l.Password)
            .NotEmpty();
    }
}
