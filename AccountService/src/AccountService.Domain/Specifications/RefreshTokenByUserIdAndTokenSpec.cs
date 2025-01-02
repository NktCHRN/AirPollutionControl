using AccountService.Domain.Models;
using Ardalis.Specification;

namespace AccountService.Domain.Specifications;
public sealed class RefreshTokenByUserIdAndTokenSpec : SingleResultSpecification<RefreshToken>
{
    public RefreshTokenByUserIdAndTokenSpec(Guid userId, string token)
    {
        Query.Where(r => r.UserId == userId && r.Token == token);
    }
}
