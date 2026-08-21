using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Basket.Commands.AddBasketItem;
using ECommerce.Application.Basket.Commands.ClearBasket;
using ECommerce.Application.Basket.Commands.RemoveBasketItem;
using ECommerce.Application.Basket.Commands.UpdateBasketItemQuantity;
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


        // Add a product to the basket

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
        .Produces<ApiResponse<GetBasketResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object?>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object?>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<object?>>(StatusCodes.Status500InternalServerError);


        // Get a basket by ID
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



        // Update the quantity of a basket item
        group.MapPut("/{basketId:guid}/items/{productId:guid}", async (
            Guid basketId,
            Guid productId,
            UpdateBasketItemQuantityRequest request,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
                {
                    var command = new UpdateBasketItemQuantityCommand(
                        basketId,
                        productId,
                        request.Quantity);

                    var result = await sender.Send(command, ct);

                    return result.ToApiResult(context);
                })
        .WithName("UpdateBasketItemQuantity")
        .WithSummary("Update the quantity of a basket item")
        .Produces<ApiResponse<GetBasketResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(StatusCodes.Status404NotFound);


        // Remove an item from the basket
        group.MapDelete("/{basketId:guid}/items/{productId:guid}", async (
            Guid basketId,
            Guid productId,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new RemoveBasketItemCommand(
                basketId,
                productId);

            var result = await sender.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("RemoveBasketItem")
        .WithSummary("Remove an item from the basket")
        .Produces<ApiResponse<GetBasketResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(
            StatusCodes.Status404NotFound);

        // Clear all items from the basket
        group.MapDelete("/{basketId:guid}", async (
            Guid basketId,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new ClearBasketCommand(basketId);

            var result = await sender.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("ClearBasket")
        .WithSummary("Clear all items from a basket")
        .Produces<ApiResponse<GetBasketResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(
            StatusCodes.Status404NotFound);



        return endpoints;
    }
}

public sealed record AddBasketItemRequest(
    Guid ProductId,
    int Quantity);

public sealed record UpdateBasketItemQuantityRequest(
    int Quantity);