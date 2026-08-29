using ECommerce.API.Responses;
using ECommerce.Application.Common.Pagination;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Types;

namespace ECommerce.API.Extensions;

public static class ResultExtensions
{
    public static IResult ToApiResult<T>(
        this Result<T> result,
        HttpContext context)
    {
        if (result.IsSuccess)
        {
            var response = ApiResponse<T>.ApiSuccess(
                result.Value,
                "Operation completed successfully.",
                context);

            return Results.Ok(response);
        }

        var errorResponse = ApiResponse<T>.ApiFailure(
            result.Errors,
            context);

        return result.Errors[0].Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(errorResponse),

            ErrorType.Validation =>
                Results.BadRequest(errorResponse),

            ErrorType.Conflict =>
                Results.Conflict(errorResponse),

            ErrorType.Unauthorized =>
                Results.Json(
                    errorResponse,
                    statusCode: StatusCodes.Status401Unauthorized),

            ErrorType.Forbidden =>
                Results.Json(
                    errorResponse,
                    statusCode: StatusCodes.Status403Forbidden),

            _ =>
                Results.Json(
                    errorResponse,
                    statusCode: StatusCodes.Status500InternalServerError)
        };
    }

    public static IResult ToApiResult<T>(
        this Result<PaginatedResult<T>> result,
        HttpContext context)
    {
        if (result.IsSuccess)
        {
            var pagination = new PaginationMeta(
                result.Value.PageNumber,
                result.Value.PageSize,
                result.Value.TotalCount);

            var response = ApiResponse<IReadOnlyList<T>>.ApiSuccess(
                result.Value.Items,
                "Operation completed successfully.",
                context,
                pagination);

            return Results.Ok(response);
        }

        var errorResponse = ApiResponse<IReadOnlyList<T>>.ApiFailure(
            result.Errors,
            context);

        return result.Errors[0].Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(errorResponse),

            ErrorType.Validation =>
                Results.BadRequest(errorResponse),

            ErrorType.Conflict =>
                Results.Conflict(errorResponse),

            ErrorType.Unauthorized =>
                Results.Json(
                    errorResponse,
                    statusCode: StatusCodes.Status401Unauthorized),

            ErrorType.Forbidden =>
                Results.Json(
                    errorResponse,
                    statusCode: StatusCodes.Status403Forbidden),

            _ =>
                Results.Json(
                    errorResponse,
                    statusCode: StatusCodes.Status500InternalServerError)
        };
    }

    public static IResult ToApiResult(
    this Result result,
    HttpContext context)
    {
        if (result.IsSuccess)
        {
            var response = ApiResponse<object?>.ApiSuccess(
                null,
                "Operation completed successfully.",
                context);

            return Results.Ok(response);
        }

        var errorResponse = ApiResponse<object?>.ApiFailure(
            result.Errors,
            context);

        return result.Errors[0].Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(errorResponse),

            ErrorType.Validation =>
                Results.BadRequest(errorResponse),

            ErrorType.Conflict =>
                Results.Conflict(errorResponse),

            ErrorType.Unauthorized =>
                Results.Json(
                    errorResponse,
                    statusCode: StatusCodes.Status401Unauthorized),

            ErrorType.Forbidden =>
                Results.Json(
                    errorResponse,
                    statusCode: StatusCodes.Status403Forbidden),

            _ =>
                Results.Json(
                    errorResponse,
                    statusCode: StatusCodes.Status500InternalServerError)
        };
    }

}