using ECommerce.API.Extensions;
using ECommerce.API.Requests.Products;
using ECommerce.API.Responses;
using ECommerce.Application.Common.Files;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products.Commands.CreateProduct;
using ECommerce.Application.Products.Commands.DeleteProduct;
using ECommerce.Application.Products.Commands.UpdateProduct;
using ECommerce.Application.Products.DTOs;
using ECommerce.Application.Products.Queries.GetAllProducts;
using ECommerce.Application.Products.Queries.GetAllProductsIncludingDeleted;
using ECommerce.Application.Products.Queries.GetDeletedProducts;
using ECommerce.Application.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(
        this IEndpointRouteBuilder endpoints)
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
        .Produces<ApiResponse<PaginatedResult<GetAllProductResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<PaginatedResult<GetAllProductResponse>>>(
            StatusCodes.Status500InternalServerError);

        // ===========================
        // Get Deleted Products
        // ===========================

        group.MapGet("/deleted", async (
            [AsParameters] PaginationRequest pagination,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetDeletedProductsQuery(pagination),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("GetDeletedProducts")
        .WithSummary("Retrieve deleted products")
        .WithDescription("Returns a paginated list of soft-deleted products.")
        .Produces<ApiResponse<PaginatedResult<GetAllProductResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<PaginatedResult<GetAllProductResponse>>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<PaginatedResult<GetAllProductResponse>>>(
            StatusCodes.Status500InternalServerError);


        // ===========================
        // Get All Products Including Deleted
        // ===========================

        group.MapGet("/all", async (
            [AsParameters] PaginationRequest pagination,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetAllProductsIncludingDeletedQuery(pagination),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("GetAllProductsIncludingDeleted")
        .WithSummary("Retrieve all products including deleted")
        .WithDescription(
            "Returns a paginated list containing both active and soft-deleted products.")
        .Produces<ApiResponse<PaginatedResult<GetAllProductResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<PaginatedResult<GetAllProductResponse>>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<PaginatedResult<GetAllProductResponse>>>(
            StatusCodes.Status500InternalServerError);

        // ===========================
        // Get Product By Id
        // ===========================

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetByIdProductsQuery(id),
                ct);

            return result.ToApiResult(context);
        })
        .WithName("GetProductById")
        .WithSummary("Retrieve a product by ID")
        .WithDescription("Returns the product matching the specified identifier.")
        .Produces<ApiResponse<GetByIdProductResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<GetByIdProductResponse>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetByIdProductResponse>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<GetByIdProductResponse>>(
            StatusCodes.Status500InternalServerError);

        // ===========================
        // Create Product
        // ===========================
        group.MapPost("/", async (
     [FromForm] CreateProductRequest request,
     IMediator mediator,
     HttpContext context,
     CancellationToken ct) =>
        {
            FileUpload? fileUpload = null;

            if (request.Picture is not null)
            {
                fileUpload = new FileUpload(
                    request.Picture.OpenReadStream(),
                    request.Picture.FileName,
                    request.Picture.ContentType);
            }

            var command = new CreateProductCommand(
                request.ProductName,
                request.ProductDescription,
                request.Price,
                fileUpload!,
                request.BrandId,
                request.TypeId);

            var result = await mediator.Send(command, ct);

            return result.ToApiResult(context);
        })
            .RequireAuthorization(policy =>
    policy.RequireRole("Admin"))
        .DisableAntiforgery()
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
            UpdateProductRequest request,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var command = new UpdateProductCommand(
                id,
                request.Name,
                request.Description,
                request.Price,
                request.BrandId,
                request.TypeId);

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
        // Update Product Picture
        // ===========================



        group.MapPut("/{id:guid}/picture", async (
    Guid id,
    [FromForm] UpdateProductPictureRequest request,
    IMediator mediator,
    HttpContext context,
    CancellationToken ct) =>
        {
            if (request.Picture is null)
            {
                return Results.BadRequest(new
                {
                    Message = "Picture is required."
                });
            }

            var fileUpload = new FileUpload(
                request.Picture.OpenReadStream(),
                request.Picture.FileName,
                request.Picture.ContentType);

            var command = new UpdateProductPictureCommand(
                id,
                fileUpload);

            var result = await mediator.Send(command, ct);

            return result.ToApiResult(context);
        })
.DisableAntiforgery()
.WithName("UpdateProductPicture")
.WithSummary("Update a product picture")
.WithDescription("Replaces the existing product picture.")
.Produces<ApiResponse<object?>>(StatusCodes.Status200OK)
.Produces<ApiResponse<object?>>(StatusCodes.Status400BadRequest)
.Produces<ApiResponse<object?>>(StatusCodes.Status404NotFound)
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