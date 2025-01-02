using Application.Attributes;
using MediatR;

namespace AccountService.Application.Features.User.Register;
[TransactionalCommand]
public sealed record RegisterCommand : IRequest<UserRegisteredDto>
{
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateOnly Birthday { get; set; }
    public Guid AgglomerationId { get; set; }
    public string Password { get; set; } = string.Empty;
}
