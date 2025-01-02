namespace AccountService.Application.Features.User.Login;
public sealed record LoginDto(string AccessToken, string RefreshToken);
