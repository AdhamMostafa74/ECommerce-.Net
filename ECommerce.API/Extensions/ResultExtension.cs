using ECommerce.API.Responses;
using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Results;

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

   

        return result.Error.Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(result.Error),

            ErrorType.Validation =>
                Results.BadRequest(result.Error),
                 
            ErrorType.Conflict =>
                Results.Conflict(result.Error),

            ErrorType.Unauthorized =>
                Results.Unauthorized(),

            ErrorType.Forbidden =>
                Results.Forbid(),

            _ =>
                Results.Problem(result.Error.Description)
        };
    }
}