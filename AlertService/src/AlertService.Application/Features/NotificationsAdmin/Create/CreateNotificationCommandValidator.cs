using FluentValidation;

namespace AlertService.Application.Features.NotificationsAdmin.Create;
public sealed class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(c => c)
            .Must(c => !string.IsNullOrWhiteSpace(c.Alert) || !string.IsNullOrWhiteSpace(c.Recommendations))
            .WithMessage("Either alert, recommendation or both must be set.");

        RuleFor(c => c.Alert)
            .MaximumLength(4000);

        RuleFor(c => c.Recommendations)
            .MaximumLength(4000);
    }
}
