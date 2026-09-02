using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Results;
using ECommerce.Domain.Common.Types;

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

        var failureMethod =
            resultType.GetMethod(
                nameof(Result<object>.Failure),
                [typeof(IEnumerable<Error>)]);

        if (failureMethod is null)
        {
            throw new InvalidOperationException(
                "Could not find Result<T>.Failure method.");
        }

        return failureMethod.Invoke(
            null,
            [errors])!;
    }
}