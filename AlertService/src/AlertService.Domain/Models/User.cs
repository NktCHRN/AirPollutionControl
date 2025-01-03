using DomainAbstractions;

namespace AlertService.Domain.Models;
public sealed class User : ISoftDeletable
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid? AgglomerationId { get; set; }                      // Can be null for global admins.
    public Agglomeration? Agglomeration { get; set; } = null!;
    public bool IsDeleted { get; set; }
}
