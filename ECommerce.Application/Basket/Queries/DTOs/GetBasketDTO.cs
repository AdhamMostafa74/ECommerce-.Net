namespace ECommerce.Application.Basket.Queries.DTOs
{
    public sealed record GetBasketResponse(
        Guid Id,
        IReadOnlyCollection<GetBasketItemResponse> Items,
        int TotalQuantity,
        decimal TotalPrice);
}