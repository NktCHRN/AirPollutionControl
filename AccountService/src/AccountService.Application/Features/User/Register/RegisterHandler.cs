using AccountService.Application.Services;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Models;
using AccountService.Domain.Specifications;
using DomainAbstractions.Exceptions;
using DotNetMessagingRepository.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using DomainAbstractions.Services;
using Microsoft.EntityFrameworkCore;

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
        var agglomeration = await agglomerationRepository.FirstOrDefaultAsync(new AgglomerationByIdSpec(request.AgglomerationId))
            ?? throw new EntityNotFoundException("Agglomeration was not found");

        if (!string.IsNullOrEmpty(request.Email))
        {
            request.Email = request.Email.Trim();
            var userByEmail = await userManager.FindByEmailAsync(request.Email);
            if (userByEmail is not null)
            {
                throw new EntityAlreadyExistsException("User with this email is already registered");
            }
        }
        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            var userByPhoneNumber = await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (userByPhoneNumber is not null)
            {
                throw new EntityAlreadyExistsException("User with this phone number is already registered");
            }
        }

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
            UserName = Guid.NewGuid().ToString(),
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
