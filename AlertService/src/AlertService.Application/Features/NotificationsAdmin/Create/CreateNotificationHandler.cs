using AlertService.Domain.Abstractions;
using AlertService.Domain.Models;
using AlertService.Domain.Specifications;
using Application.Abstractions;
using DomainAbstractions.Exceptions;
using DotNetMessagingRepository.Common;
using MediatR;

namespace AlertService.Application.Features.NotificationsAdmin.Create;
public sealed record CreateNotificationHandler : IRequestHandler<CreateNotificationCommand>
{
    private readonly ICurrentApplicationUserService currentApplicationUserService;
    private readonly IRepository<Notification> notificationRepository;
    private readonly IRepository<User> userRepository;
    private readonly IRepository<Country> countryRepository;
    private readonly IRepository<Agglomeration> agglomerationRepository;
    private readonly TimeProvider timeProvider;

    public CreateNotificationHandler(ICurrentApplicationUserService currentApplicationUserService, IRepository<Notification> notificationRepository, IRepository<User> userRepository, IRepository<Country> countryRepository, IRepository<Agglomeration> agglomerationRepository, TimeProvider timeProvider)
    {
        this.currentApplicationUserService = currentApplicationUserService;
        this.notificationRepository = notificationRepository;
        this.userRepository = userRepository;
        this.countryRepository = countryRepository;
        this.agglomerationRepository = agglomerationRepository;
        this.timeProvider = timeProvider;
    }

    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var userId = currentApplicationUserService.Id ?? throw new EntityNotFoundException("User or refresh token was not found");

        var notification = new Notification
        {
            SenderId = userId,
            Alert = request.Alert,
            Recommendations = request.Recommendations,
            CreatedAt = timeProvider.GetUtcNow()
        };

        if (currentApplicationUserService.IsInRole(Roles.AgglomerationGovernmentMember) || currentApplicationUserService.IsInRole(Roles.AgglomerationAdmin))
        {
            notification.Scope = Domain.Enums.NotificationScope.Agglomeration;

            var agglomeration = await agglomerationRepository.FirstOrDefaultAsync(new AgglomerationForNotificationCreationSpec(currentApplicationUserService.AgglomerationId.GetValueOrDefault()))
                ?? throw new EntityNotFoundException("Agglomeration was not found");
            notification.OrganizationName = agglomeration.AdministrationName;

            var usersIds = await userRepository.ListAsync(new AgglomerationUsersSpec(agglomeration.Id), cancellationToken);
            foreach (var targetUserId in usersIds)
            {
                notification.UserNotifications.Add(new UserNotification
                {
                    UserId = targetUserId,
                });
            }
        }
        else if (currentApplicationUserService.IsInRole(Roles.CountryGovernmentMember) || currentApplicationUserService.IsInRole(Roles.CountryAdmin))
        {
            notification.Scope = Domain.Enums.NotificationScope.Country;

            var country = await countryRepository.FirstOrDefaultAsync(new CountryForNotificationCreationSpec(currentApplicationUserService.CountryId.GetValueOrDefault()))
                ?? throw new EntityNotFoundException("Country was not found");
            notification.OrganizationName = country.AdministrationName;

            var usersIds = await userRepository.ListAsync(new CountryUsersSpec(country.Id), cancellationToken);
            foreach (var targetUserId in usersIds)
            {
                notification.UserNotifications.Add(new UserNotification
                {
                    UserId = targetUserId,
                });
            }
        }
        else
        {
            throw new ForbiddenForUserException("Not enough permissions to do this action.");
        }

        await notificationRepository.AddAsync(notification, cancellationToken);
    }
}
