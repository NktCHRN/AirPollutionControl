namespace AspNetCore.Contracts;
public record ErrorResponse
{
    public string ErrorMessage { get; set; } = string.Empty;
}
