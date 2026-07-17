namespace ECommerce.API.Responses;

public sealed class PaginationMeta
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }

    public bool HasNextPage => PageIndex < TotalPages;

    public bool HasPreviousPage => PageIndex > 1;
}