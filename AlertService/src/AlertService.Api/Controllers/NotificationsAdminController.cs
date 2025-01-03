using AlertService.Api.Contracts.Requests.NotificationsAdmin;
using AlertService.Application.Features.NotificationsAdmin.Create;
using AspNetCore.Contracts;
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

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
    {
        var command = new CreateNotificationCommand { Alert = request.Alert, Recommendations = request.Recommendations };

        await mediator.Send(command);

        return NoContentResponse();
    }
}
