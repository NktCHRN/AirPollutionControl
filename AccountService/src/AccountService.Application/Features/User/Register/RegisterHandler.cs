using AccountService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Features.User.Register;
public sealed class RegisterHandler : IRequestHandler<RegisterCommand, UserRegisteredDto>
{
    private readonly UserManager<AccountService.Domain.Models.User> userManager;

    public async Task<UserRegisteredDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
