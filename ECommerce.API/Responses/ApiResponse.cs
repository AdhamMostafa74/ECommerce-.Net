using ECommerce.Domain.Common;

namespace ECommerce.API.Responses;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public T? Data { get; init; }

    public IReadOnlyList<Error>? Errors { get; init; }

    public ApiMeta Meta { get; init; } = new();

    public static ApiResponse<T> ApiSuccess(
        T data,
        string message,
        HttpContext context,
        PaginationMeta? pagination = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = null,
            Meta = new ApiMeta
            {
                TraceId = context.TraceIdentifier,
                Pagination = pagination
            }
        };
    }

    public static ApiResponse<T> ApiFailure(
        IReadOnlyList<Error> errors,
        HttpContext context)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = errors.Count > 0
                ? errors[0].Description
                : "An error occurred.",
            Data = default,
            Errors = errors,
            Meta = new ApiMeta
            {
                TraceId = context.TraceIdentifier
            }
        };
    }
}