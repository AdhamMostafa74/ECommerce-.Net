using ECommerce.API.Extensions;
using ECommerce.API.Requests.Types;
using ECommerce.API.Responses;
using ECommerce.Application.Common.Pagination;
using ECommerce.Application.Types.Commands.CreateType;
using ECommerce.Application.Types.Commands.DeleteType;
using ECommerce.Application.Types.Commands.UpdateType;
using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Types.Queries.GetAllTypes;
using ECommerce.Application.Types.Queries.GetAllTypesIncludingDeleted;
using ECommerce.Application.Types.Queries.GetDeletedTypes;
using ECommerce.Application.Types.Queries.GetTypeById;
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
        // Get Active Types
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
        .WithSummary("Retrieve all active product types")
        .WithDescription(
            "Returns a paginated list of active product types.")
        .Produces<ApiResponse<
            PaginatedResult<GetAllTypesResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<
            PaginatedResult<GetAllTypesResponse>>>(
            StatusCodes.Status500InternalServerError);


        // ===========================
        // Get Deleted Types
        // ===========================

        group.MapGet("/deleted", async (
            [AsParameters] PaginationRequest pagination,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetDeletedTypesQuery(pagination),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("GetDeletedTypes")
        .WithSummary("Retrieve deleted product types")
        .WithDescription(
            "Returns a paginated list of soft-deleted product types.")
        .Produces<ApiResponse<
            PaginatedResult<GetAllTypesResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<
            PaginatedResult<GetAllTypesResponse>>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<
            PaginatedResult<GetAllTypesResponse>>>(
            StatusCodes.Status500InternalServerError);


        // ===========================
        // Get All Types Including Deleted
        // ===========================

        group.MapGet("/all", async (
            [AsParameters] PaginationRequest pagination,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetAllTypesIncludingDeletedQuery(
                    pagination),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("GetAllTypesIncludingDeleted")
        .WithSummary(
            "Retrieve all product types including deleted")
        .WithDescription(
            "Returns a paginated list containing both active and soft-deleted product types.")
        .Produces<ApiResponse<
            PaginatedResult<GetAllTypesResponse>>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<
            PaginatedResult<GetAllTypesResponse>>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<
            PaginatedResult<GetAllTypesResponse>>>(
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
        .WithDescription(
            "Returns the active product type matching the specified identifier.")
        .Produces<ApiResponse<GetByIdTypeResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<GetByIdTypeResponse>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetByIdTypeResponse>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<GetByIdTypeResponse>>(
            StatusCodes.Status500InternalServerError);


        // ===========================
        // Create Type
        // ===========================

        group.MapPost("/", async (
            CreateTypeRequest request,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new CreateTypeCommand(request.Name),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("CreateType")
        .WithSummary("Create a product type")
        .WithDescription(
            "Creates a new product type.")
        .Produces<ApiResponse<GetAllTypesResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<GetAllTypesResponse>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetAllTypesResponse>>(
            StatusCodes.Status409Conflict)
        .Produces<ApiResponse<GetAllTypesResponse>>(
            StatusCodes.Status500InternalServerError);


        // ===========================
        // Update Type
        // ===========================

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateTypeRequest request,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new UpdateTypeCommand(
                    id,
                    request.Name),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("UpdateType")
        .WithSummary("Update a product type")
        .WithDescription(
            "Updates an existing active product type.")
        .Produces<ApiResponse<GetAllTypesResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<GetAllTypesResponse>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<GetAllTypesResponse>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<GetAllTypesResponse>>(
            StatusCodes.Status409Conflict)
        .Produces<ApiResponse<GetAllTypesResponse>>(
            StatusCodes.Status500InternalServerError);


        // ===========================
        // Delete Type
        // ===========================

        group.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new DeleteTypeCommand(id),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("Admin"))
        .WithName("DeleteType")
        .WithSummary("Delete a product type")
        .WithDescription(
            "Soft-deletes the specified product type.")
        .Produces<ApiResponse<Unit>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<Unit>>(
            StatusCodes.Status404NotFound)
        .Produces<ApiResponse<Unit>>(
            StatusCodes.Status500InternalServerError);

        return endpoints;
    }
}