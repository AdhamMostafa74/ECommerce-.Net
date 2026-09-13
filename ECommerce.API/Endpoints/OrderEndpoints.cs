using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Orders.Commands.CreateOrder;
using ECommerce.Application.Orders.Queries.GetMyOrders;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/orders")
            .WithTags("Orders")
            .RequireAuthorization();

        group.MapGet("/", async (
   [AsParameters] PaginationRequest pagination,
    ISender sender,
    HttpContext context,
    CancellationToken ct) =>
        {
            var result = await sender.Send(
                new GetMyOrdersQuery(pagination),
                ct);

            return result.ToApiResult(context);
        })
.WithName("GetMyOrders")
.WithSummary("Get the current user's orders")
.WithDescription(
    "Returns the authenticated user's orders using pagination.")
.Produces<ApiResponse<PaginatedResult<GetMyOrdersResponse>>>(
    StatusCodes.Status200OK)
.Produces<ApiResponse<PaginatedResult<GetMyOrdersResponse>>>(
    StatusCodes.Status400BadRequest)
.Produces<ApiResponse<PaginatedResult<GetMyOrdersResponse>>>(
    StatusCodes.Status404NotFound);

        group.MapPost("/", async (
            CreateOrderRequest request,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new CreateOrderCommand(
                request.Street,
                request.City,
                request.State,
                request.PostalCode,
                request.Country);

            var result = await sender.Send(
                command,
                ct);

            return result.ToApiResult(context);
        })
        .WithName("CreateOrder")
        .WithSummary("Create an order from the current user's basket")
        .WithDescription(
            "Creates an order using the authenticated user's basket, customer profile, and supplied shipping details.")
        .Produces<ApiResponse<Guid>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<Guid>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Guid>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<Guid>>(
            StatusCodes.Status409Conflict)
        .Produces<ApiResponse<Guid>>(
            StatusCodes.Status500InternalServerError);

        return endpoints;
    }
}

public sealed record CreateOrderRequest(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country);