using AccountService.Api.Contracts.Responses.Agglomerations;
using AccountService.Application.Features.Agglomerations.Search;
using AspNetCore.Contracts;
using AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public sealed class AgglomerationsController : BaseController
{
    private readonly IMediator mediator;

    public AgglomerationsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<AgglomerationResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchCountries([FromQuery] Guid countryId, [FromQuery] SearchPaginationParametersRequest searchParameters)
    {
        var query = new SearchAgglomerationsQuery(countryId, searchParameters.PerPage, searchParameters.Page, searchParameters.SearchText);

        var dto = await mediator.Send(query);

        return OkResponse(new PagedResponse<AgglomerationResponse>(dto.Data.Select(c => new AgglomerationResponse(c.Id, c.Name, c.AdministrationName)), dto.TotalCount));
    }
}
