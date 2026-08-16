using ECommerce.Domain.Common;

namespace ECommerce.API.Responses;

public sealed class ApiResponse<T>
{
    public bool Success { get; init; }

    public string Message { get; init; } = string.Empty;

    public T? Data { get; init; }

    public Error? Error { get; init; }

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
            Error = null,
            Meta = new ApiMeta
            {
                TraceId = context.TraceIdentifier,
                Pagination = pagination
            }
        };
    }

    public static ApiResponse<T> ApiFailure(
        Error error,
        HttpContext context)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = error.Description,
            Data = default,
            Error = error,
            Meta = new ApiMeta
            {
                TraceId = context.TraceIdentifier
            }
        };
    }
}