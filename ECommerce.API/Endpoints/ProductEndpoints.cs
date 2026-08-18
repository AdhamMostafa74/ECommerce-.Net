using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products;
using ECommerce.Application.Products.Commands.CreateProduct;
using ECommerce.Application.Products.Commands.DeleteProduct;
using ECommerce.Application.Products.Commands.UpdateProduct;
using ECommerce.Application.Products.DTOs;
using ECommerce.Application.Products.Queries.GetAllProducts;
using ECommerce.Application.Products.Queries.GetProductById;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/products")
            .WithTags("Products");

        // ===========================
        // Get All Products
        // ===========================

        group.MapGet("/", async (
      [AsParameters] PaginationRequest pagination,
      IMediator mediator,
      HttpContext context,
      CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetAllProductsQuery(pagination),
                ct);

            return result.ToApiResult(context);
        })
        .WithName("GetProducts")
        .WithSummary("Retrieve all products")
        .WithDescription("Returns a paginated list of available products.")
        .Produces<ApiResponse<IReadOnlyList<GetAllProductResponse>>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<IReadOnlyList<GetAllProductResponse>>>(StatusCodes.Status500InternalServerError);

        // ===========================
        // Get Product By Id
        // ===========================

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetByIdProductsQuery(id), ct);

            return result.ToApiResult(context);
        })
        .WithName("GetProductById")
        .WithSummary("Retrieve a product by ID")
        .WithDescription("Returns the product matching the specified identifier.")
        .Produces<ApiResponse<GetByIdProductResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<GetByIdProductResponse>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetByIdProductResponse>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<GetByIdProductResponse>>(StatusCodes.Status500InternalServerError);

        // ===========================
        // Create Product
        // ===========================

        group.MapPost("/", async (
            CreateProductCommand command,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("CreateProduct")
        .WithSummary("Create a product")
        .WithDescription("Creates a new product.")
        .Produces<ApiResponse<Guid>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status409Conflict)
        .Produces<ApiResponse<Guid>>(StatusCodes.Status500InternalServerError);

        // ===========================
        // Update Product
        // ===========================

        group.MapPatch("/{id:guid}", async (
            Guid id,
            UpdateProductCommand body,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new UpdateProductCommand(
                id,
                body.Name,
                body.Description,
                body.PictureUrl,
                body.Price,
                body.BrandId,
                body.TypeId);

            var result = await mediator.Send(command, ct);

            return result.ToApiResult(context);
        })
        .WithName("UpdateProduct")
        .WithSummary("Update a product")
        .WithDescription("Updates one or more fields of an existing product.")
        .Produces<ApiResponse<object?>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object?>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object?>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<object?>>(StatusCodes.Status409Conflict)
        .Produces<ApiResponse<object?>>(StatusCodes.Status500InternalServerError);

        // ===========================
        // Delete Product
        // ===========================

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new DeleteProductCommand(id),
                ct);

            return result.ToApiResult(context);
        })
        .WithName("DeleteProduct")
        .WithSummary("Delete a product")
        .WithDescription("Deletes the specified product.")
        .Produces<ApiResponse<object?>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<object?>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<object?>>(StatusCodes.Status500InternalServerError);

        return endpoints;
    }
}