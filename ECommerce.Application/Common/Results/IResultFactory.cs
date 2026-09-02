using ECommerce.Domain.Common;

namespace ECommerce.Application.Common.Results;

public interface IResultFactory
{
    object CreateFailure(
        Type resultType,
        IReadOnlyList<Error> errors);
}