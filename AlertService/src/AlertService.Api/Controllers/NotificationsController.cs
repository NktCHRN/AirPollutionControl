using AlertService.Api.Contracts.Enums;
using AlertService.Api.Contracts.Responses.Notifications;
using AlertService.Application.Features.Notifications.GetPaged;
using AspNetCore.Contracts;
using AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlertService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController : BaseController
{
    private readonly IMediator mediator;

    public NotificationsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<UserNotificationResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPaged([FromQuery] PaginationParametersRequest parameters)
    {
        var query = new GetPagedNotificationsQuery(parameters.PerPage, parameters.Page);

        var dto = await mediator.Send(query);

        return OkResponse(new PagedResponse<UserNotificationResponse>(
            dto.Data.Select(c => new UserNotificationResponse
            {
                Id = c.Id,
                Scope = Enum.Parse<NotificationScope>(c.Scope.ToString()),
                Alert = c.Alert,
                Recommendations = c.Recommendations,
                OrganizationName = c.OrganizationName,
                CreatedAt = c.CreatedAt,
                IsRead = c.IsRead
            }), 
            dto.TotalCount));
    }
}
