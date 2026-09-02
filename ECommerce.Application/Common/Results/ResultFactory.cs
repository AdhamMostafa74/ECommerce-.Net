using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common;

namespace ECommerce.Application.Common.Results;

public sealed class ResultFactory : IResultFactory
{
    public object CreateFailure(
        Type resultType,
        IReadOnlyList<Error> errors)
    {
        if (!resultType.IsGenericType)
        {
            throw new InvalidOperationException(
                $"Expected a generic Result type, but received {resultType.Name}.");
        }

        var genericTypeDefinition =
            resultType.GetGenericTypeDefinition();

        if (genericTypeDefinition != typeof(Result<>))
        {
            throw new InvalidOperationException(
                $"Expected Result<T>, but received {resultType.Name}.");
        }

        var genericArgument =
            resultType.GetGenericArguments()[0];

        var failureMethod =
            genericTypeDefinition.GetMethod(
                nameof(Result<object>.Failure),
                [typeof(IEnumerable<Error>)]);

        if (failureMethod is null)
        {
            throw new InvalidOperationException(
                "Could not find Result<T>.Failure method.");
        }

        var genericFailureMethod =
            failureMethod.MakeGenericMethod(genericArgument);

        return genericFailureMethod.Invoke(
            null,
            [errors])!;
    }
}