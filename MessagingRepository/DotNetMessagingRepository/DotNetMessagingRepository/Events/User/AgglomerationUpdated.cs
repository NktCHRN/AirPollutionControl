namespace DotNetMessagingRepository.Events.User;
public sealed record AgglomerationUpdated
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
}
