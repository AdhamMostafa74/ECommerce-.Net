using ECommerce.Domain.Common.Types;

namespace ECommerce.Domain.Common.Results;

public sealed class Result<T>
{
    private readonly T? _value;

    private Result(
        T? value,
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

        _value = value;
        IsSuccess = isSuccess;
        Errors = errors;
        SuccessType = successType;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<Error> Errors { get; }

    public SuccessType SuccessType { get; }

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

    public static Result<T> Failure(Error error)
        => new(
            default,
            false,
            [error],
            SuccessType.Accepted);

    public static Result<T> Failure(IEnumerable<Error> errors)
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