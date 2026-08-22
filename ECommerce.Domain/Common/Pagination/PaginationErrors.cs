using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.Application.Common.Pagination;

public static class PaginationErrors
{
    public static readonly Error InvalidPageNumber =
        new(
            "Pagination.InvalidPageNumber",
            "Page number must be greater than 0.",
            ErrorType.Validation);

    public static readonly Error InvalidPageSize =
        new(
            "Pagination.InvalidPageSize",
            "Page size must be between 1 and 100.",
            ErrorType.Validation);

    public static readonly Error PageNumberOutOfRange =
        new(
            "Pagination.PageNumberOutOfRange",
            "The requested page does not exist.",
            ErrorType.Validation);
}