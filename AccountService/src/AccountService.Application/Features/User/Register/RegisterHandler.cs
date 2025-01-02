using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AccountService.Application.Features.User.Register;
public sealed class RegisterHandler : IRequestHandler<RegisterCommand, UserRegisteredDto>
{
    private readonly UserManager<Domain.Models.User> userManager;

    public async Task<UserRegisteredDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
