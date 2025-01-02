﻿using AccountService.Api.Contracts.Requests.User;
using AccountService.Api.Contracts.Responses.User;
using AccountService.Application.Features.User.Register;
using AspNetCore.Contracts;
using AspNetCore.Controllers;
using MediatR;
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
}
