namespace AccountService.Api.Contracts.Responses.User;

public sealed record TokensResponse(string AccessToken, string RefreshToken);
