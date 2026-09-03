using ECommerce.Application.Brands.Errors;
using ECommerce.Application.Brands.DTOs;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.BrandsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Brands.Commands.UpdateBrand;

public sealed class UpdateBrandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<
        UpdateBrandCommand,
        Result<BrandResponse>>
{
    public async Task<Result<BrandResponse>> Handle(
        UpdateBrandCommand request,
        CancellationToken ct)
    {
        var repository = unitOfWork.Repository<ProductBrand>();

        var brand = await repository.FirstOrDefaultAsync(
            new BrandByIdSpecification(request.Id),
            ct);

        if (brand is null)
            return Result<BrandResponse>.Failure(
                BrandErrors.NotFound);

        var nameExists = await repository.AnyAsync(
            new BrandByNameSpecification(
                request.Name,
                request.Id),
            ct);

        if (nameExists)
            return Result<BrandResponse>.Failure(
                BrandErrors.AlreadyExists);

        brand.Rename(request.Name);

        repository.Update(brand);

        await unitOfWork.SaveChangesAsync(ct);

        return Result<BrandResponse>.Success(
            new BrandResponse(
                brand.Id,
                brand.Name));
    }
}