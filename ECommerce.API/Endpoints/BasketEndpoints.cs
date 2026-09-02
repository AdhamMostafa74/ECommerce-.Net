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

        // Add a product to the current user's basket

        group.MapPost("/items", async (
            AddBasketItemRequest request,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new AddBasketItemCommand(
                request.ProductId,
                request.Quantity);

            var result = await mediator.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("AddBasketItem")
        .WithSummary("Add a product to the current user's basket")
        .WithDescription(
            "Adds a product to the basket belonging to the authenticated user.")
        .Produces<ApiResponse<GetBasketResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<object?>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object?>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<object?>>(
            StatusCodes.Status500InternalServerError);

        // Get the current user's basket

        group.MapGet("/", async (
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var query = new GetBasketQuery();

            var result = await sender.Send(query, ct);

            return result.ToApiResult(context);
        })
         .RequireAuthorization()
        .WithName("GetBasket")
        .WithSummary("Get the current user's basket")
        .Produces<ApiResponse<GetBasketResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(
            StatusCodes.Status404NotFound);

        // Update the quantity of a basket item

        group.MapPut("/items/{productId:guid}", async (
            Guid productId,
            UpdateBasketItemQuantityRequest request,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new UpdateBasketItemQuantityCommand(
                productId,
                request.Quantity);

            var result = await sender.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("UpdateBasketItemQuantity")
        .WithSummary("Update the quantity of a basket item")
        .Produces<ApiResponse<GetBasketResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(
            StatusCodes.Status404NotFound);

        // Remove an item from the current user's basket

        group.MapDelete("/items/{productId:guid}", async (
            Guid productId,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new RemoveBasketItemCommand(
                productId);

            var result = await sender.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("RemoveBasketItem")
        .WithSummary("Remove an item from the current user's basket")
        .Produces<ApiResponse<GetBasketResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<object>>(
            StatusCodes.Status404NotFound);

        // Clear the current user's basket

        group.MapDelete("/", async (
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new ClearBasketCommand();

            var result = await sender.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("ClearBasket")
        .WithSummary("Clear the current user's basket")
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