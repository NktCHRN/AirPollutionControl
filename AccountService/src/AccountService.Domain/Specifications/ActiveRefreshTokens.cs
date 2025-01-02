using AccountService.Domain.Models;
using Ardalis.Specification;

namespace AccountService.Domain.Specifications;
public sealed class ActiveRefreshTokens : Specification<RefreshToken>
{
    public ActiveRefreshTokens(Guid userId, DateTimeOffset currentTime)
    {
        Query.Where(r => r.UserId == userId && r.ExpiryTime >= currentTime);
    }
}
