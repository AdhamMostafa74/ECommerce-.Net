using ECommerce.Application.Brands.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.BrandsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Brands.Commands.DeleteBrand;

public sealed class DeleteBrandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBrandCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(
        DeleteBrandCommand request,
        CancellationToken ct)
    {
        var repository = unitOfWork.Repository<ProductBrand>();

        var brand = await repository.FirstOrDefaultAsync(
            new BrandByIdSpecification(request.Id),
            ct);

        if (brand is null)
        {
            return Result<Unit>.Failure(
                BrandErrors.NotFound);
        }

        brand.DeleteBrand();

        await unitOfWork.SaveChangesAsync(ct);

        return Result<Unit>.Success(Unit.Value);
    }
}