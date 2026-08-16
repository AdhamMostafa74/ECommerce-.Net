

using ECommerce.Application.Products.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.ProductsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Products.Commands.DeleteProduct;

public sealed class DeleteProductHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle
       (DeleteProductCommand request,
       CancellationToken cancellationToken)
    {
        var productRepository = _unitOfWork.Repository<Product>();

        var product = await productRepository.FirstOrDefaultAsync(
            new ProductByIdSpecification(request.Id)
            , cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        productRepository.Delete(product);

        await _unitOfWork.SaveChangeAsync(cancellationToken);


        return Result.Success();

    }
}
