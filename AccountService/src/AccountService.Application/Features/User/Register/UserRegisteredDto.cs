namespace AccountService.Application.Features.User.Register;
public sealed record UserRegisteredDto(Guid Id, string Name, string? Email, string? PhoneNumber);
