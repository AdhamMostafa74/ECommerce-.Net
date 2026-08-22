using ECommerce.Domain.Common.Results;

namespace ECommerce.Application.Common.Pagination;

public static class PaginationValidator
{
    public static Result Validate(PaginationRequest pagination)
    {
        if (pagination.PageNumber < 1)
        {
            return Result.Failure(PaginationErrors.InvalidPageNumber);
        }

        if (pagination.PageSize < 1 ||
            pagination.PageSize > PaginationRequest.MaxPageSize)
        {
            return Result.Failure(PaginationErrors.InvalidPageSize);
        }

        return Result.Success();
    }
}