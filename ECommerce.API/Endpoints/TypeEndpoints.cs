using ECommerce.Application.Types;

namespace ECommerce.API.Endpoints
{
    public static class TypeEndpoints
    {
        public static IEndpointRouteBuilder MapTypeEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints
                .MapGroup("/api/v1/types")
                .WithTags("Types");

            group.MapGet("/", async (
                ITypeQueryService typeQueryService,
                CancellationToken ct) =>
            {
                var types = await typeQueryService.GetAllTypeResponse(ct);

                return Results.Ok(types);
            })
            .WithName("GetTypes")
            .WithSummary("Gets all types");

            group.MapGet("/{id:guid}", async (
                Guid id,
                ITypeQueryService typeQueryService,
                CancellationToken ct) =>
            {
                var type = await typeQueryService.GetByIdTypeResponse(id, ct);

                return type is null
                    ? Results.NotFound()
                    : Results.Ok(type);
            })
            .WithName("GetTypeById")
            .WithSummary("Gets a type by id");

            return endpoints;
        }
    }
}