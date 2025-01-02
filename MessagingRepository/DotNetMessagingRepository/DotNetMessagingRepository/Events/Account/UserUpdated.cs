namespace DotNetMessagingRepository.Events.Account;
public sealed record UserUpdated
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Patronymic { get; set; }
    public DateOnly Birthday { get; set; }
    public bool IsConfirmed { get; set; }
    public bool IsRestricted { get; set; }
    public string? RestrictionNote { get; set; }
    public DateTimeOffset? RestrictionEnd { get; set; }
    public string? PositionName { get; set; }
    public Guid AgglomerationId { get; set; }
}
