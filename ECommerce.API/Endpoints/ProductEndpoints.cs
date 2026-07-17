using ECommerce.Application.Products;

namespace ECommerce.API.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/products")
            .WithTags("Products");

        group.MapGet("/", async (
            IProductQueryService productQueryService,
            CancellationToken ct) =>
        {
            var products = await productQueryService.GetAllProductResponse(ct);

            return Results.Ok(products);
        })
        .WithName("GetProducts")
        .WithSummary("Gets all products")
        .WithDescription("Returns all products.");

        group.MapGet("/{id:guid}", async (
            Guid id,
            IProductQueryService productQueryService,
            CancellationToken ct) =>
        {
            var product = await productQueryService.GetByIdProductResponse(id, ct);

            return product is null
                ? Results.NotFound()
                : Results.Ok(product);
        })
        .WithName("GetProductById")
        .WithSummary("Gets a product by id")
        .WithDescription("Returns the requested product.");

        return endpoints;
    }
}