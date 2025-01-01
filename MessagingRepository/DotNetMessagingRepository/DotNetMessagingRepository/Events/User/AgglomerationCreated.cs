namespace DotNetMessagingRepository.Events.User;
public sealed record AgglomerationCreated
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
}
