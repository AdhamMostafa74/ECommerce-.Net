using ECommerce.Application.Brands.Errors;
using ECommerce.Application.Brands.DTOs;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.BrandsSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Brands.Commands.CreateBrand;

public sealed class CreateBrandHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<
        CreateBrandCommand,
        Result<BrandResponse>>
{
    public async Task<Result<BrandResponse>> Handle(
        CreateBrandCommand request,
        CancellationToken ct)
    {
        var repository = unitOfWork.Repository<ProductBrand>();

        var exists = await repository.AnyAsync(
            new BrandByNameSpecification(request.Name),
            ct);

        if (exists)
            return Result<BrandResponse>.Failure(
                BrandErrors.AlreadyExists);

        var brand = ProductBrand.Create(
            Guid.NewGuid(),
            request.Name);

        repository.Create(brand);

        await unitOfWork.SaveChangesAsync(ct);

        var response = new BrandResponse(
            brand.Id,
            brand.Name);

        return Result<BrandResponse>.Success(response);
    }
}