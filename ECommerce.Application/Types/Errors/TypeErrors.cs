using ECommerce.Domain.Common;

namespace ECommerce.Application.Types.Errors;

public static class TypeErrors
{
    public static readonly Error NotFound = new(
        "Type.NotFound",
        "The requested type was not found." 
        ,ErrorType.NotFound);

    public static readonly Error AlreadyExists = new(
        "Type.AlreadyExists",
        "A type with the same name already exists.",
        ErrorType.Conflict);
}