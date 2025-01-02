using AccountService.Api.Contracts.Responses.Countries;
using AccountService.Application.Features.Countries.Search;
using AspNetCore.Contracts;
using AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public sealed class CountriesController : BaseController
{
    private readonly IMediator mediator;

    public CountriesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<CountryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchCountries([FromQuery] SearchPaginationParametersRequest searchParameters)
    {
        var query = new SearchCountriesQuery(searchParameters.PerPage, searchParameters.Page, searchParameters.SearchText);

        var dto = await mediator.Send(query);

        return OkResponse(new PagedResponse<CountryResponse>(dto.Data.Select(c => new CountryResponse(c.Id, c.Name, c.AdministrationName)), dto.TotalCount));
    }
}
