using ECommerce.API.Extensions;
using ECommerce.API.Requests.Brands;
using ECommerce.API.Responses;
using ECommerce.Application.Brands.Commands.CreateBrand;
using ECommerce.Application.Brands.Commands.DeleteBrand;
using ECommerce.Application.Brands.Commands.UpdateBrand;
using ECommerce.Application.Brands.DTOs;
using ECommerce.Application.Brands.Queries.GetAllBrands;
using ECommerce.Application.Brands.Queries.GetAllBrandsIncludingDeleted;
using ECommerce.Application.Brands.Queries.GetBrandById;
using ECommerce.Application.Brands.Queries.GetDeletedBrands;
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


        // ===========================
        // Create Brand
        // ===========================

        group.MapPost("/", async (
            CreateBrandRequest request,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new CreateBrandCommand(request.Name),
                ct);

            return result.ToApiResult(context);
        })
        .WithName("CreateBrand")
        .WithSummary("Create a new brand")
        .WithDescription("Creates a new product brand.")
        .Produces<ApiResponse<BrandResponse>>(
            StatusCodes.Status201Created)
        .Produces<ApiResponse<BrandResponse>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<BrandResponse>>(
            StatusCodes.Status409Conflict)
        .Produces<ApiResponse<BrandResponse>>(
            StatusCodes.Status500InternalServerError);


        // ===========================
        // Update Brand
        // ===========================


        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateBrandRequest request,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new UpdateBrandCommand(
                    id,
                    request.Name),
                ct);

            return result.ToApiResult(context);
        })
        .WithName("UpdateBrand")
        .WithSummary("Update a brand")
        .WithDescription("Updates the name of an existing product brand.")
        .Produces<ApiResponse<BrandResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<BrandResponse>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<BrandResponse>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<BrandResponse>>(
            StatusCodes.Status409Conflict)
        .Produces<ApiResponse<BrandResponse>>(
            StatusCodes.Status500InternalServerError);

        // ===========================
        // Get Active Brands
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
        .WithSummary("Retrieve all active brands")
        .WithDescription(
            "Returns a paginated list of active product brands.")
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status500InternalServerError);


        // ===========================
        // Get Deleted Brands
        // ===========================

        group.MapGet("/deleted", async (
            [AsParameters] PaginationRequest pagination,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetDeletedBrandsQuery(pagination),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("GetDeletedBrands")
        .WithSummary("Retrieve deleted brands")
        .WithDescription(
            "Returns a paginated list of soft-deleted product brands.")
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status500InternalServerError);


        // ===========================
        // Get All Brands Including Deleted
        // ===========================

        group.MapGet("/all", async (
            [AsParameters] PaginationRequest pagination,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetAllBrandsIncludingDeletedQuery(pagination),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("GetAllBrandsIncludingDeleted")
        .WithSummary("Retrieve all brands including deleted")
        .WithDescription(
            "Returns a paginated list containing both active and soft-deleted brands.")
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<PaginatedResult<GetAllBrandsResponse>>>(
            StatusCodes.Status500InternalServerError);



        // ===========================
        // Delete Brand
        // ===========================

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new DeleteBrandCommand(id),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("DeleteBrand")
        .WithSummary("Delete a brand")
        .WithDescription(
            "Soft-deletes the specified product brand.")
        .Produces<ApiResponse<Unit>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<Unit>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<Unit>>(
            StatusCodes.Status500InternalServerError);
        return endpoints;
    }
}