using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Services;
public static class IdentityUtilities
{
    public static string IdentityFailuresToString(IEnumerable<IdentityError> failures)
    {
        return string.Join(Environment.NewLine, failures.Select(f => f.Description));
    }
}
