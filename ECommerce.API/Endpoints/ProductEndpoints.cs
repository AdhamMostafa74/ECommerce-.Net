using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Products;
using ECommerce.Application.Products.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/products")
            .WithTags("Products");

        group.MapGet("/", async (
           [AsParameters] PaginationRequest pagination,
            IProductQueryService productQueryService,
            HttpContext context,
            CancellationToken ct) =>
        {
            var response = await productQueryService.GetAllProductResponse(pagination, ct);

            return response.ToApiResult(context);
        })
        .WithName("GetProducts")
        .WithSummary("Retrieve all products")
        .WithDescription("Returns a paginated list of available products.")
        .Produces<ApiResponse<IReadOnlyList<GetAllProductResponse>>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<IReadOnlyList<GetAllProductResponse>>>(StatusCodes.Status500InternalServerError);

        group.MapGet("/{id:guid}", async (
            Guid id,
            IProductQueryService productQueryService,
            HttpContext context,
            CancellationToken ct) =>
        {
            var response = await productQueryService.GetByIdProductResponse(id, ct);

            return response.ToApiResult(context);
        })
        .WithName("GetProductById")
        .WithSummary("Retrieve a product by ID")
        .WithDescription("Returns the product matching the specified identifier.")
        .Produces<ApiResponse<GetByIdProductResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<GetByIdProductResponse>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetByIdProductResponse>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<GetByIdProductResponse>>(StatusCodes.Status500InternalServerError);

        return endpoints;
    }
}