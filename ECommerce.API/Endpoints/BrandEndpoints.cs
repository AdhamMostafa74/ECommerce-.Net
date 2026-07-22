using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Brands;
using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Common.Pagination;
using Microsoft.AspNetCore.Mvc;


namespace ECommerce.API.Endpoints;

public static class BrandEndpoints
{
    public static IEndpointRouteBuilder MapBrandEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/brands")
            .WithTags("Brands");

        group.MapGet("/", async (
            [AsParameters] PaginationRequest paginationRequest,
            IBrandQueryService brandQueryService,
            HttpContext context,
            CancellationToken ct) =>
        {
            var response = await brandQueryService.GetAllBrandResponse(paginationRequest, ct);

            return response.ToApiResult(context);
        })
        .WithName("GetBrands")
        .WithSummary("Retrieve all brands")
        .WithDescription("Returns a list of all available product brands.")
        .Produces<ApiResponse<IReadOnlyList<GetAllBrandsResponse>>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<GetAllBrandsResponse>>(StatusCodes.Status500InternalServerError);

        group.MapGet("/{id:guid}", async (
            Guid id,
            IBrandQueryService brandQueryService,
            HttpContext context,
            CancellationToken ct) =>
        {
            var response = await brandQueryService.GetByIdBrandResponse(id, ct);

            return response.ToApiResult(context);
        })
        .WithName("GetBrandById")
        .WithSummary("Retrieve a brand by ID")
        .WithDescription("Returns the brand matching the specified identifier.")
        .Produces<ApiResponse<GetByIdBrandResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<GetByIdBrandResponse>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetByIdBrandResponse>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<GetByIdBrandResponse>>(StatusCodes.Status500InternalServerError);
        

        return endpoints;
    }
}