using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;

namespace ECommerce.Domain.Common.Pagination;

public static class PaginationValidator
{
    public static Result<Unit> Validate(PaginationRequest pagination)
    {
        if (pagination.PageNumber < 1)
        {
            return Result<Unit>.Failure(
                PaginationErrors.InvalidPageNumber);
        }

        if (pagination.PageSize < 1 ||
            pagination.PageSize > PaginationRequest.MaxPageSize)
        {
            return Result<Unit>.Failure(
                PaginationErrors.InvalidPageSize);
        }

        return Result<Unit>.Success(Unit.Value);
    }
}