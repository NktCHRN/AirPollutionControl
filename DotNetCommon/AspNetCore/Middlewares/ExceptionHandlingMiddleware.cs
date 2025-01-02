using AspNetCore.Contracts;
using DomainAbstractions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AspNetCore.Middlewares;
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            var (statusCode, message) = ex switch
            {
                EntityAlreadyExistsException => (HttpStatusCode.Conflict, ex.Message),
                EntityNotFoundException => (HttpStatusCode.NotFound, ex.Message),
                EntityValidationFailedException => (HttpStatusCode.BadRequest, ex.Message),
                UserUnauthorizedException => (HttpStatusCode.Unauthorized, ex.Message),
                ForbiddenForUserException => (HttpStatusCode.Forbidden, ex.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occured on the server.")
            };

            if (statusCode is HttpStatusCode.InternalServerError)
            {
                _logger.LogError("An unexpected error occured: {ex}", ex);
            }

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse(message));
        }
    }
}