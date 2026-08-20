using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Basket.Commands.AddBasketItem;
using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Application.Basket.Queries.GetBasket;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class BasketEndpoints
{
    public static IEndpointRouteBuilder MapBasketEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/basket")
            .WithTags("BasketEntity");

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


        group.MapGet("/{basketId:guid}", async (
            Guid basketId,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var query = new GetBasketQuery(basketId);

            var result = await sender.Send(query, ct);

            return result.ToApiResult(context);
        })
        .WithName("GetBasket")
        .WithSummary("Get a basket by ID")
        .Produces<ApiResponse<GetBasketResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);



        return endpoints;
    }
}

public sealed record AddBasketItemRequest(
    Guid ProductId,
    int Quantity);