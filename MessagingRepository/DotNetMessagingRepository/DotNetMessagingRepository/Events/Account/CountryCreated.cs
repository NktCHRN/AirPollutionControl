namespace DotNetMessagingRepository.Events.Account;
public sealed record CountryCreated
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AdministrationName { get; set; } = string.Empty;
}
