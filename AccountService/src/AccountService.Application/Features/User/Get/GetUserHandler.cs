using Application.Abstractions;
using DomainAbstractions.Exceptions;
using DotNetMessagingRepository.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.User.Get;
public sealed class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly UserManager<Domain.Models.User> userManager;
    private readonly ICurrentApplicationUserService currentApplicationUserService;

    public GetUserHandler(UserManager<Domain.Models.User> userManager, ICurrentApplicationUserService currentApplicationUserService)
    {
        this.userManager = userManager;
        this.currentApplicationUserService = currentApplicationUserService;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var userId = currentApplicationUserService.Id ?? throw new EntityNotFoundException("User was not found");

        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken: cancellationToken) ?? throw new EntityNotFoundException("User was not found");

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
            Birthday = user.Birthday,
            AgglomerationId = user.AgglomerationId,
            AgglomerationName = user.Agglomeration?.Name,
            CountryId = user.Agglomeration?.CountryId,
            CountryName = user.Agglomeration?.Country?.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsConfirmed = currentApplicationUserService.IsConfirmed,
            IsRestricted = currentApplicationUserService.IsRestricted,
            RestrictionEnd = user.RestrictionEnd,
            RestrictionNote = user.RestrictionNote,
            PositionName = user.PositionName,
            OrganizationName = GetOrganizationName(user)
        };
    }

    private string? GetOrganizationName(Domain.Models.User user)
    {
        if (currentApplicationUserService.IsInRole(Roles.AgglomerationAdmin) || currentApplicationUserService.IsInRole(Roles.AgglomerationGovernmentMember))
        {
            return user.Agglomeration?.AdministrationName ?? string.Empty;
        }

        if (currentApplicationUserService.IsInRole(Roles.CountryAdmin) || currentApplicationUserService.IsInRole(Roles.CountryGovernmentMember))
        {
            return user.Agglomeration?.Country.Name ?? string.Empty;
        }

        return null;
    }
}
