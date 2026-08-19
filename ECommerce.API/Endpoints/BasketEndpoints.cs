using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Basket.Commands.AddBasketItem;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class BasketEndpoints
{
    public static IEndpointRouteBuilder MapBasketEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/basket")
            .WithTags("Basket");

        group.MapPost("/{basketId:guid}/items", async (
            Guid basketId,
            AddBasketItemRequest request,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new AddBasketItemCommand(
                basketId,
                request.ProductId,
                request.Quantity);

            var result = await mediator.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("AddBasketItem")
        .WithSummary("Add a product to the basket")
        .WithDescription("Adds a product to the specified basket.")
        .Produces<ApiResponse<object?>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object?>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object?>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<object?>>(StatusCodes.Status500InternalServerError);

        return endpoints;
    }
}

public sealed record AddBasketItemRequest(
    Guid ProductId,
    int Quantity);