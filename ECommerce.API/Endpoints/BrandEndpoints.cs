using ECommerce.Application.Brands;

namespace ECommerce.API.Endpoints
{
    public static class BrandEndpoints
    {
        public static IEndpointRouteBuilder MapBrandEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints
                .MapGroup("/api/v1/brands")
                .WithTags("Brands");

            group.MapGet("/", async (
                IBrandQueryService brandQueryService,
                CancellationToken ct) =>
            {
                var brands = await brandQueryService.GetAllBrandResponse(ct);

                return Results.Ok(brands);
            })
            .WithName("GetBrands")
            .WithSummary("Gets all brands");

            group.MapGet("/{id:guid}", async (
                Guid id,
                IBrandQueryService brandQueryService,
                CancellationToken ct) =>
            {
                var brand = await brandQueryService.GetByIdBrandResponse(id, ct);

                return brand is null
                    ? Results.NotFound()
                    : Results.Ok(brand);
            })
            .WithName("GetBrandById")
            .WithSummary("Gets a brand by id");

            return endpoints;
        }
    }
}