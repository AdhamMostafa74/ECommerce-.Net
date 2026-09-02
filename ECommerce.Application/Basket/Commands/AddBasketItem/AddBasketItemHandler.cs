using ECommerce.Application.Basket.Queries.DTOs;
using ECommerce.Application.Common.Identity;
using ECommerce.Application.Products.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.ProductsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Basket;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Basket.Commands.AddBasketItem;

public sealed class AddBasketItemHandler(
    IRepository<Product> productRepository,
    IBasketRepository basketRepository,
    ICurrentUser currentUser)
    : IRequestHandler<AddBasketItemCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(
        AddBasketItemCommand request,
        CancellationToken ct)
    {
        var product = await productRepository.FirstOrDefaultAsync(
            new ProductByIdSpecification(request.ProductId),
            ct);

        if (product is null)
            return Result<GetBasketResponse>.Failure(
                ProductErrors.NotFound);

        var basket = await basketRepository.GetAsync(
            currentUser.UserId,
            ct);

        basket ??= new BasketEntity(Guid.NewGuid());

        var item = new BasketItem(
            product.Id,
            product.Name,
            product.PictureUrl,
            product.Price,
            request.Quantity);

        basket.AddItem(item);

        await basketRepository.SaveAsync(
            currentUser.UserId,
            basket,
            ct);

        var response = new GetBasketResponse(
            basket.Id,
            basket.Items.Select(item =>
                new GetBasketItemResponse(
                    item.ProductId,
                    item.ProductName,
                    item.PictureUrl,
                    item.Price,
                    item.Quantity))
                .ToList(),
            basket.TotalQuantity,
            basket.TotalPrice);

        return Result<GetBasketResponse>.Success(response);
    }
}