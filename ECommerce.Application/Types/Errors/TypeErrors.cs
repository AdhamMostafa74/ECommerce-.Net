using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.Application.Types.Errors;

public static class TypeErrors
{
    public static readonly Error NotFound = new(
        "ProductType.NotFound",
        "The requested type was not found." 
        ,ErrorType.NotFound);

    public static readonly Error AlreadyExists = new(
        "ProductType.AlreadyExists",
        "A type with the same name already exists.",
        ErrorType.Conflict);
}