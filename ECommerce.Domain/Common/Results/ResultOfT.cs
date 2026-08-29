using ECommerce.Domain.Common.Types;

namespace ECommerce.Domain.Common.Results;

public sealed class Result<T> : Result
{
    private readonly T? _value;

    internal Result(
        T? value,
        bool isSuccess,
        IReadOnlyList<Error> errors,
        SuccessType successType)
        : base(isSuccess, errors, successType)
    {
        _value = value;
    }

    public T Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "Cannot access value of a failed result.");

    public static Result<T> Success(T value)
        => new(
            value,
            true,
            [],
            SuccessType.Accepted);

    public static new Result<T> Failure(Error error)
        => new(
            default,
            false,
            new[] { error },
            SuccessType.Accepted);

    public static new Result<T> Failure(IEnumerable<Error> errors)
        => new(
            default,
            false,
            [.. errors],
            SuccessType.Accepted);

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<IReadOnlyList<Error>, TResult> onFailure)
    {
        return IsSuccess
            ? onSuccess(Value)
            : onFailure(Errors);
    }

    public override string ToString()
    {
        return IsSuccess
            ? $"Success ({Value})"
            : $"Failure: {string.Join(
                ", ",
                Errors.Select(e => $"({e.Code}): {e.Description}"))}";
    }

    public static implicit operator Result<T>(T value)
        => Success(value);
}