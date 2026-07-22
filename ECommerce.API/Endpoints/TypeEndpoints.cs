using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types;
using ECommerce.Application.Types.DTOs;

namespace ECommerce.API.Endpoints;

public static class TypeEndpoints
{
    public static IEndpointRouteBuilder MapTypeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/types")
            .WithTags("Types");

        group.MapGet("/", async (
            [AsParameters]PaginationRequest paginationRequest ,
            ITypeQueryService typeQueryService,
            HttpContext context,
            CancellationToken ct) =>
        {
            var response = await typeQueryService.GetAllTypeResponse(paginationRequest, ct);

            return response.ToApiResult(context);
        })
        .WithName("GetTypes")
        .WithSummary("Retrieve all product types")
        .WithDescription("Returns a list of all available product types.")
        .Produces<ApiResponse<IEnumerable<GetAllTypesResponse>>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<IEnumerable<GetAllTypesResponse>>>(StatusCodes.Status500InternalServerError);

        group.MapGet("/{id:guid}", async (
            Guid id,
            ITypeQueryService typeQueryService,
            HttpContext context,
            CancellationToken ct) =>
        {
            var response = await typeQueryService.GetByIdTypeResponse(id, ct);

            return response.ToApiResult(context);
        })
        .WithName("GetTypeById")
        .WithSummary("Retrieve a product type by ID")
        .WithDescription("Returns the product type matching the specified identifier.")
        .Produces<ApiResponse<GetByIdTypeResponse>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<GetByIdTypeResponse>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetByIdTypeResponse>>(StatusCodes.Status404NotFound)
        .Produces<ApiResponse<GetByIdTypeResponse>>(StatusCodes.Status500InternalServerError);

        return endpoints;
    }
}