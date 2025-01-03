using AlertService.Domain.Abstractions;
using AlertService.Domain.Models;
using AlertService.Domain.Specifications;
using MediatR;

namespace AlertService.Application.Features.Users.SaveUser;
public sealed class SaveUserHandler : IRequestHandler<SaveUserCommand>
{
    private readonly IRepository<User> userRepository;

    public SaveUserHandler(IRepository<User> userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task Handle(SaveUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(new UserByIdSpec(request.Id));

        if (user is not null)
        {
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.AgglomerationId = request.AgglomerationId;

            await userRepository.UpdateAsync(user, cancellationToken);
        }
        else
        {
            user = new User
            {
                Id = request.Id
            };
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.AgglomerationId = request.AgglomerationId;
            await userRepository.AddAsync(user, cancellationToken);
        }
    }
}
