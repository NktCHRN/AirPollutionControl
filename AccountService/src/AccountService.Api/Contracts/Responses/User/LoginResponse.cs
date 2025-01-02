namespace AccountService.Api.Contracts.Responses.User;

public sealed record LoginResponse(string AccessToken, string RefreshToken);
