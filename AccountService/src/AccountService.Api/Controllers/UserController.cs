using AccountService.Api.Contracts.Requests.User;
using AccountService.Api.Contracts.Responses.User;
using AccountService.Application.Features.User.Get;
using AccountService.Application.Features.User.GetRoles;
using AccountService.Application.Features.User.Login;
using AccountService.Application.Features.User.RefreshTokens;
using AccountService.Application.Features.User.Register;
using AccountService.Application.Features.User.RevokeAllTokens;
using AccountService.Application.Features.User.RevokeToken;
using AspNetCore.Contracts;
using AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : BaseController
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AccountRegisteredResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterCommand
        {
            AgglomerationId = request.AgglomerationId,
            Birthday = request.Birthday,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            Password = request.Password,
            Patronymic = request.Patronymic,
            PhoneNumber = request.PhoneNumber,
        };

        var dto = await mediator.Send(command);

        return OkResponse(new AccountRegisteredResponse(dto.Id, dto.Name, dto.Email, dto.PhoneNumber));
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.Login, request.Password);

        var dto = await mediator.Send(command);

        return OkResponse(new LoginResponse(dto.AccessToken, dto.RefreshToken));
    }

    [HttpPost("tokens/refresh")]
    [ProducesResponseType(typeof(ApiResponse<TokensResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshTokens([FromBody] RefreshTokensRequest request)
    {
        var command = new RefreshTokensCommand(request.AccessToken, request.RefreshToken);

        var dto = await mediator.Send(command);

        return OkResponse(new TokensResponse(dto.AccessToken, dto.RefreshToken));
    }

    [Authorize]
    [HttpPost("tokens/revoke")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
    {
        var command = new RevokeTokenCommand(request.RefreshToken);

        await mediator.Send(command);

        return NoContentResponse();
    }

    [Authorize]
    [HttpPost("tokens/revoke-all")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RevokeAllActiveTokens()
    {
        var command = new RevokeAllTokensCommand();

        await mediator.Send(command);

        return NoContentResponse();
    }

    [Authorize]
    [HttpGet()]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        var command = new GetUserQuery();

        var dto = await mediator.Send(command);

        return OkResponse(new UserResponse
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            LastName = dto.LastName,
            Patronymic = dto.Patronymic,
            Birthday = dto.Birthday,
            AgglomerationId = dto.AgglomerationId,
            AgglomerationName = dto.AgglomerationName,
            CountryId = dto.CountryId,
            CountryName = dto.CountryName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            IsConfirmed = dto.IsConfirmed,
            IsRestricted = dto.IsRestricted,
            RestrictionEnd = dto.RestrictionEnd,
            RestrictionNote = dto.RestrictionNote,
            PositionName = dto.PositionName,
            OrganizationName = dto.OrganizationName
        });
    }

    [Authorize]
    [HttpGet("roles")]
    [ProducesResponseType(typeof(ApiResponse<string[]>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRoles()
    {
        var command = new GetUserRolesQuery();

        var dto = await mediator.Send(command);

        return OkResponse(dto.Roles);
    }
}
