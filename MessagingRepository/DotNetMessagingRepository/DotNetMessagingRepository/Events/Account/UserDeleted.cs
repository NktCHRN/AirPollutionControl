namespace DotNetMessagingRepository.Events.Account;
public sealed record UserDeleted
{
    public Guid Id { get; set; }
}
