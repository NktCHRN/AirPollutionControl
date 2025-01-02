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
    private readonly TimeProvider timeProvider;

    public RegisterHandler(UserManager<Domain.Models.User> userManager, IRepository<Agglomeration> agglomerationRepository, TimeProvider timeProvider)
    {
        this.userManager = userManager;
        this.agglomerationRepository = agglomerationRepository;
        this.timeProvider = timeProvider;
    }

    public async Task<UserRegisteredDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var agglomeration = await agglomerationRepository.FirstOrDefaultAsync(new AgglomerationByIdSpec(request.AgglomerationId))
            ?? throw new EntityNotFoundException("Agglomeration was not found");

        if (!string.IsNullOrEmpty(request.Email))
        {
            request.Email = request.Email.Trim();
            var emailIsTaken = await userManager.Users.AnyAsync(u => u.Email == request.Email, cancellationToken: cancellationToken);
            if (emailIsTaken)
            {
                throw new EntityAlreadyExistsException("User with this email is already registered");
            }
        }
        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            var phoneNumberIsTaken = await userManager.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken: cancellationToken);
            if (phoneNumberIsTaken)
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
            CreatedAt = timeProvider.GetUtcNow(),
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
