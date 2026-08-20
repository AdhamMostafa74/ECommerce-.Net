namespace ECommerce.Application.Basket.Queries.DTOs
{
    public sealed record GetBasketItemResponse(
        Guid ProductId,
        string ProductName,
        string PictureUrl,
        decimal Price,
        int Quantity);
}