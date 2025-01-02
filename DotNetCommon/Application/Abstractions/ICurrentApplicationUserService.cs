namespace Application.Abstractions;
public interface ICurrentApplicationUserService
{
    Guid? Id { get; }
    string? UserName { get; }
    string? Email { get; }
    string? PhoneNumber { get; }
    string? FullName { get; }
    Guid? AgglomerationId { get; }
    string? AgglomerationName { get; }
    Guid? CountryId { get; }
    string? CountryName { get; }
    Guid GetValidatedId();
    bool IsConfirmed { get; }
    bool IsRestricted { get; }
    bool IsInRole(string role);
}
