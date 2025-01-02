namespace AccountService.Api.Contracts.Responses.User;

public sealed record AccountRegisteredResponse(Guid Id, string Name, string Email, string PhoneNumber);
