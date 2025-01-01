using DomainAbstractions;

namespace AccountService.Domain.Models;
public sealed class Tenant : ISoftDeletable
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
    public Country Country { get; set; } = null!;
    public bool IsDeleted { get; set; }
}
