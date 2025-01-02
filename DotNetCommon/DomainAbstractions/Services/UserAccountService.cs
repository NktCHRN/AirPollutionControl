namespace DomainAbstractions.Services;
public sealed class UserAccountService
{
    public static string GetFullName(string firstName, string? middleName, string lastName, string? patronymic)
    {
        return string.Join(" ", new string?[] { firstName, middleName, lastName, patronymic }.Where(s => !string.IsNullOrEmpty(s)));
    }
}
