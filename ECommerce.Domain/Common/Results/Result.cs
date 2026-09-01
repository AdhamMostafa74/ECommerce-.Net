using ECommerce.Domain.Common.Types;

namespace ECommerce.Domain.Common.Results;

public class Result
{
    protected Result(
        bool isSuccess,
        IReadOnlyList<Error> errors,
        SuccessType successType)
    {
        if (isSuccess && errors.Count > 0)
            throw new InvalidOperationException(
                "A successful result cannot contain errors.");

        if (!isSuccess && errors.Count == 0)
            throw new InvalidOperationException(
                "A failure result must contain at least one error.");

        IsSuccess = isSuccess;
        Errors = errors;
        SuccessType = successType;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<Error> Errors { get; }

    public SuccessType SuccessType { get; }

    public static Result Success()
        => new(
            true,
            [],
            SuccessType.Accepted);

    public static Result Failure(Error error)
        => new(
            false,
            [error],
            SuccessType.Accepted);

    public static Result Failure(IEnumerable<Error> errors)
        => new(
            false,
            [.. errors],
            SuccessType.Accepted);

    public override string ToString()
    {
        return IsSuccess
            ? "Success"
            : $"Failure: {string.Join(
                ", ",
                Errors.Select(e => $"({e.Code}): {e.Description}"))}";
    }
}