using Application.Attributes;
using MediatR;

namespace AlertService.Application.Features.Users.SaveUser;
[TransactionalCommand]
public sealed record SaveUserCommand : IRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? AgglomerationId { get; set; }
}
