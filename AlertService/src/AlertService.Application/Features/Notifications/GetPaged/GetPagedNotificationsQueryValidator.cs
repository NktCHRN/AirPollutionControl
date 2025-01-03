using Application.Validators;
using FluentValidation;

namespace AlertService.Application.Features.Notifications.GetPaged;
public sealed class GetPagedNotificationsQueryValidator : AbstractValidator<GetPagedNotificationsQuery>
{
    public GetPagedNotificationsQueryValidator()
    {
        Include(new BasePagedQueryValidator());
    }
}
