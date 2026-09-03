using ECommerce.Application.Types.Errors;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Specifications.TypesSpecifications;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.Application.Types.Commands.DeleteType;

public sealed class DeleteTypeHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<
        DeleteTypeCommand,
        Result<Unit>>
{
    public async Task<Result<Unit>> Handle(
        DeleteTypeCommand request,
        CancellationToken ct)
    {
        var repository =
            unitOfWork.Repository<ProductType>();

        var type = await repository.FirstOrDefaultAsync(
            new ProductTypeByIdSpecification(request.Id),
            ct);

        if (type is null)
        {
            return Result<Unit>.Failure(
                TypeErrors.NotFound);
        }

        type.DeleteType();

        await unitOfWork.SaveChangesAsync(ct);

        return Result<Unit>.Success(Unit.Value);
    }
}