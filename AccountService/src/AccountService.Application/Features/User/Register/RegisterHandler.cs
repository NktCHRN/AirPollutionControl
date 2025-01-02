using AccountService.Application.Services;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Models;
using AccountService.Domain.Specifications;
using DomainAbstractions.Exceptions;
using DotNetMessagingRepository.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using DomainAbstractions.Services;

namespace AccountService.Application.Features.User.Register;
public sealed class RegisterHandler : IRequestHandler<RegisterCommand, UserRegisteredDto>
{
    private readonly UserManager<Domain.Models.User> userManager;
    private readonly IRepository<Agglomeration> agglomerationRepository;

    public RegisterHandler(UserManager<Domain.Models.User> userManager, IRepository<Agglomeration> agglomerationRepository)
    {
        this.userManager = userManager;
        this.agglomerationRepository = agglomerationRepository;
    }

    public async Task<UserRegisteredDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var username = !string.IsNullOrWhiteSpace(request.Email) ? request.Email : request.PhoneNumber;

        var agglomeration = await agglomerationRepository.FirstOrDefaultAsync(new AgglomerationByIdSpec(request.AgglomerationId))
            ?? throw new EntityNotFoundException("Agglomeration was not found");

        var user = new Domain.Models.User
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Patronymic = request.Patronymic,
            Birthday = request.Birthday,
            Agglomeration = agglomeration,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserName = username,
        };

        var userCreationResults = await userManager.CreateAsync(user, request.Password);
        if (!userCreationResults.Succeeded)
        {
            throw new EntityValidationFailedException(IdentityUtilities.IdentityFailuresToString(userCreationResults.Errors));
        }

        var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.User);
        if (!addToRoleResult.Succeeded)
        {
            throw new EntityValidationFailedException(IdentityUtilities.IdentityFailuresToString(userCreationResults.Errors));
        }

        return new UserRegisteredDto(user.Id, UserAccountService.GetFullName(user.FirstName, user.MiddleName, user.LastName, user.Patronymic), user.Email, user.PhoneNumber);
    }
}
