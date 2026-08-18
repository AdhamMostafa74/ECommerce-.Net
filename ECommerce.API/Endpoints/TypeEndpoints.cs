using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Types.Queries.GetAllTypes;
using ECommerce.Application.Types.Queries.GetTypeById;
using ECommerce.Application.Types.Querires.GetTypeById;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class TypeEndpoints
{
    public static IEndpointRouteBuilder MapTypeEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/types")
            .WithTags("Types");

        // ===========================
        // Get All Types
        // ===========================

        group.MapGet("/", async (
            [AsParameters] PaginationRequest pagination,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetAllTypesQuery(pagination),
                ct);

            return result.ToApiResult(context);
        })
        .WithName("GetTypes")
        .WithSummary("Retrieve all product types")
        .WithDescription("Returns a paginated list of all available product types.")
        .Produces<ApiResponse<PaginatedResult<GetAllTypesResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<PaginatedResult<GetAllTypesResponse>>>(
            StatusCodes.Status500InternalServerError);

        // ===========================
        // Get Type By Id
        // ===========================

        group.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetTypeByIdQuery(id),
                ct);

            return result.ToApiResult(context);
        })
        .WithName("GetTypeById")
        .WithSummary("Retrieve a product type by ID")
        .WithDescription("Returns the product type matching the specified identifier.")
        .Produces<ApiResponse<GetByIdTypeResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<GetByIdTypeResponse>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetByIdTypeResponse>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<GetByIdTypeResponse>>(
            StatusCodes.Status500InternalServerError);

        return endpoints;
    }
}