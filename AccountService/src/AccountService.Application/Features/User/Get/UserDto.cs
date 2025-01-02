namespace AccountService.Application.Features.User.Get;
public sealed record UserDto
{
    public required Guid Id { get; set; }
    public required string FirstName { get; set; } = string.Empty;
    public required string? MiddleName { get; set; }
    public required string LastName { get; set; } = string.Empty;
    public required string? Email { get; set; }
    public required string? PhoneNumber { get; set; }
    public required string? Patronymic { get; set; }
    public required DateOnly Birthday { get; set; }
    public required bool IsConfirmed { get; set; }
    public required bool IsRestricted { get; set; }
    public required string? RestrictionNote { get; set; }
    public required DateTimeOffset? RestrictionEnd { get; set; }
    public string? PositionName { get; set; }
    public string? OrganizationName { get; set; }
    public required Guid? AgglomerationId { get; set; }
    public required string? AgglomerationName { get; set; }
    public required Guid? CountryId { get; set; }
    public required string? CountryName { get; set; }
}
