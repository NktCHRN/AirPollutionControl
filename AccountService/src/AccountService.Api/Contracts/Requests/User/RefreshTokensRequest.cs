namespace AccountService.Api.Contracts.Requests.User;

public sealed record RefreshTokensRequest(string AccessToken, string RefreshToken);
