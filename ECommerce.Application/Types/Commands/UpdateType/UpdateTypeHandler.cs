using ECommerce.Application.Types.DTOs;
using ECommerce.Application.Types.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.TypesSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Types.Commands.UpdateType;

public sealed class UpdateTypeHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<
        UpdateTypeCommand,
        Result<GetAllTypesResponse>>
{
    public async Task<Result<GetAllTypesResponse>>
        Handle(
            UpdateTypeCommand request,
            CancellationToken ct)
    {
        var repository =
            unitOfWork.Repository<ProductType>();

        var type = await repository.FirstOrDefaultAsync(
            new ProductTypeByIdSpecification(request.Id),
            ct);

        if (type is null)
        {
            return Result<GetAllTypesResponse>.Failure(
                TypeErrors.NotFound);
        }

        var exists = await repository.AnyAsync(
            new ProductTypeByNameSpecification(
                request.Name,
                request.Id),
            ct);

        if (exists)
        {
            return Result<GetAllTypesResponse>.Failure(
                TypeErrors.AlreadyExists);
        }

        type.Rename(request.Name);

        repository.Update(type);

        await unitOfWork.SaveChangesAsync(ct);

        return Result<GetAllTypesResponse>.Success(
            new GetAllTypesResponse(
                type.Id,
                type.Name));
    }
}