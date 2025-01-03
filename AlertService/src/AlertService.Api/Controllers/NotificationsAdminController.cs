using AspNetCore.Controllers;
using DotNetMessagingRepository.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlertService.Api.Controllers;
[Route("api/admin/notifications")]
[ApiController]
[Authorize(Roles = $"{Roles.AgglomerationGovernmentMember},{Roles.AgglomerationAdmin},{Roles.CountryGovernmentMember},{Roles.CountryAdmin}")]
public class NotificationsAdminController : BaseController
{
    private readonly IMediator mediator;

    public NotificationsAdminController(IMediator mediator)
    {
        this.mediator = mediator;
    }
}
