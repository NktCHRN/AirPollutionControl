using Application.Abstractions;
using DomainAbstractions.Exceptions;
using DotNetMessagingRepository.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AspNetCore.Services;
public class CurrentApplicationUserService : ICurrentApplicationUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentApplicationUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? Id => ParseGuid(ClaimTypes.NameIdentifier, _httpContextAccessor.HttpContext?.User);

    private static Guid? ParseGuid(string claimType, ClaimsPrincipal? user)
    {
        if (Guid.TryParse(user?.FindFirst(claimType)?.Value, out Guid parsed))
            return parsed;
        return null;
    }

    public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public string? FullName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName);

    public string? PhoneNumber => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.MobilePhone);

    public Guid? AgglomerationId => ParseGuid("AgglomerationId", _httpContextAccessor.HttpContext?.User);

    public string? AgglomerationName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("AgglomerationName");

    public Guid? CountryId => ParseGuid("CountryId", _httpContextAccessor.HttpContext?.User);

    public string? CountryName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Country);

    public bool IsConfirmed => IsInRole(Roles.ConfirmedUser);

    public bool IsRestricted => IsInRole(Roles.RestrictedUser);

    public Guid GetValidatedId()
    {
        return Id ?? throw new UserUnauthorizedException("User is either not authorized or error retrieving id from claim.");
    }

    public bool IsInRole(string role)
    {
        return _httpContextAccessor.HttpContext?.User?.IsInRole(role) is true;
    }
}
