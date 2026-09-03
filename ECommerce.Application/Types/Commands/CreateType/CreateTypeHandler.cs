using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Types.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.TypesSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Types.Commands.CreateType;

public sealed class CreateTypeHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<
        CreateTypeCommand,
        Result<GetAllTypesResponse>>
{
    public async Task<Result<GetAllTypesResponse>>
        Handle(
            CreateTypeCommand request,
            CancellationToken ct)
    {
        var repository =
            unitOfWork.Repository<ProductType>();

        var exists = await repository.AnyAsync(
            new ProductTypeByNameSpecification(request.Name),
            ct);

        if (exists)
        {
            return Result<GetAllTypesResponse>.Failure(
                TypeErrors.AlreadyExists);
        }

        var type = ProductType.Create(
            Guid.NewGuid(),
            request.Name);

        repository.Create(type);

        await unitOfWork.SaveChangesAsync(ct);

        return Result<GetAllTypesResponse>.Success(
            new GetAllTypesResponse(
                type.Id,
                type.Name));
    }
}