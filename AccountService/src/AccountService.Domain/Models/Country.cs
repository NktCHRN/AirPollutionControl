namespace AccountService.Domain.Models;
public sealed class Country
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
