namespace ECommerce.Domain.Common.Results;

public sealed class Result<T> : Result
{
    private readonly T? _value;

    internal Result(T? value, bool isSuccess, Error error, SuccessType successType)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException("Cannot access value of a failed result.");



    public static Result<T> Success(T value)
        => new(value, true, Error.None, SuccessType.Accepted);

    public static new Result<T> Failure(Error error)
        => new(default!, false, error, SuccessType.Accepted);

    public TResult Match<TResult>(
    Func<T, TResult> onSuccess,
    Func<Error, TResult> onFailure)
    {
        return IsSuccess
            ? onSuccess(Value)
            : onFailure(Error);
    }

    public override string ToString()
    {
        return IsSuccess
            ? $"Success ({Value})"
            : $"Failure ({Error.Code}): {Error.Description}";
    }

    public static implicit operator Result<T>(T value) => Success(value);
}