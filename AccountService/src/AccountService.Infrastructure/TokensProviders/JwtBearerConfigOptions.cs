namespace AccountService.Infrastructure.TokensProviders;
public sealed class JwtBearerConfigOptions
{
    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int LifetimeInMinutes { get; set; }
}
