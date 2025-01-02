namespace AccountService.Application.Options;
public sealed class TokenProvidersOptions
{
    public int PasswordResetLifetimeInMinutes { get; set; }
    public int EmailConfirmationLifetimeInHours { get; set; }
    public int RefreshTokenLifetimeInDays { get; set; }
}
