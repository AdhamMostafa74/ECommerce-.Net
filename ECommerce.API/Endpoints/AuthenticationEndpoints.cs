using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Authentication.Commands.Login;
using ECommerce.Application.Authentication.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/auth")
            .WithTags("Authentication");

        //Login

        group.MapPost("/login", async (
            LoginCommand command,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("Login")
        .WithSummary("Authenticate a user")
        .WithDescription(
            "Authenticates a user using email and password and returns a JWT access token.")
        .Produces<ApiResponse<string>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<string>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<string>>(StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<string>>(StatusCodes.Status500InternalServerError);

        //Registration

                group.MapPost("/register", async (
            RegisterCommand command,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
                {
                    var result = await mediator.Send(command, ct);

                    return result.ToApiResult(context);
                })
        .WithName("Register")
        .WithSummary("Register a new user")
        .WithDescription("Creates a new user account.")
        .Produces<ApiResponse<Guid>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status409Conflict)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status500InternalServerError);


                return endpoints;
    }
}