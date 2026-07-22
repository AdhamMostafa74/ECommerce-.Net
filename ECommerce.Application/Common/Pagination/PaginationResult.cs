namespace ECommerce.Application.Common.Pagination;

public sealed record PaginatedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }

    public required int PageNumber { get; init; }

    public required int PageSize { get; init; }

    public required int TotalCount { get; init; }

    public int TotalPages =>
        (int)Math.Ceiling((double)TotalCount / PageSize);
}