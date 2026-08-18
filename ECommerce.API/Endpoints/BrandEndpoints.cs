using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Brands.Queries.GetAllBrands;
using ECommerce.Application.Brands.Queries.GetBrandById;
using ECommerce.Application.Common.Pagination;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class BrandEndpoints
{
    public static IEndpointRouteBuilder MapBrandEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/brands")
            .WithTags("Brands");

        // ===========================
        // Get All Brands
        // ===========================

        group.MapGet("/", async (
            [AsParameters] PaginationRequest pagination,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetAllBrandsQuery(pagination),
                ct);

            return result.ToApiResult(context);
        })
        .WithName("GetBrands")
        .WithSummary("Retrieve all brands")
        .WithDescription("Returns a paginated list of all available product brands.")
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status500InternalServerError);

        // ===========================
        // Get Brand By Id
        // ===========================

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetBrandByIdQuery(id),
                ct);

            return result.ToApiResult(context);
        })
        .WithName("GetBrandById")
        .WithSummary("Retrieve a brand by ID")
        .WithDescription("Returns the brand matching the specified identifier.")
        .Produces<ApiResponse<GetByIdBrandResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<GetByIdBrandResponse>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetByIdBrandResponse>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<GetByIdBrandResponse>>(
            StatusCodes.Status500InternalServerError);

        return endpoints;
    }
}