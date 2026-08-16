using ECommerce.Application.Brands.Errors;
using ECommerce.Application.Products.Errors;
using ECommerce.Application.Types.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.BrandsSpecifications;
using ECommerce.Domain.Common.Specifications.TypesSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;

using MediatR;

namespace ECommerce.Application.Products.Commands.CreateProduct;

public sealed class CreateProductHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var productRepository = _unitOfWork.Repository<Product>();
        var brandRepository = _unitOfWork.Repository<ProductBrand>();
        var typeRepository = _unitOfWork.Repository<ProductType>();

        // ---------- Business Validation ----------

        var brandExists = await brandRepository.AnyAsync(
            new BrandByIdSpecification(request.BrandId),
            cancellationToken);

        if (!brandExists)
            return Result<Guid>.Failure(BrandErrors.AlreadyExists);

        var typeExists = await typeRepository.AnyAsync(
            new ProductTypeByIdSpecification(request.TypeId),
            cancellationToken);

        if (!typeExists)
            return Result<Guid>.Failure(TypeErrors.AlreadyExists);

        var productExists = await productRepository.AnyAsync(
            new ProductNameSpecification(request.ProductName),
            cancellationToken);

        if (productExists)
            return Result<Guid>.Failure(ProductErrors.ProductAlreadyExists);

        // ---------- Create Entity ----------

        var product = Product.Create(
            request.ProductName,
            request.ProductDescription,
            request.PictureUrl,
            request.Price,
            request.BrandId,
            request.TypeId);

        // ---------- Persist ----------

        productRepository.Create(product);

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        // ---------- Result ----------

        return Result<Guid>.Success(product.Id);
    }
}