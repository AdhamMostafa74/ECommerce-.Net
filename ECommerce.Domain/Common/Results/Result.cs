using ECommerce.Domain.Common.Types;

namespace ECommerce.Domain.Common.Results;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException(
              "A successful result cannot contain an error.");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException(
              "A Failure result cannot contain a value.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public SuccessType SuccessType { get; }

    public static Result Success()
        => new(true, Error.None);

    public static Result Failure(Error error)
        => new(false, error);


    public override string ToString()
    {
        return IsSuccess
            ? "Success"
            : $"Failure ({Error.Code}): {Error.Description}";
    }
}