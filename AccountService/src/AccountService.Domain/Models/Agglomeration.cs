namespace AccountService.Domain.Models;
public sealed class Agglomeration
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AdministrationName { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
    public Country Country { get; set; } = null!;
}
