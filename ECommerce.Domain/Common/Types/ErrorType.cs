namespace ECommerce.Domain.Common.Types
{
    public enum ErrorType
    {
        None,
        Validation,
        NotFound,
        Conflict,
        Unauthorized,
        Forbidden,
        Failure,
        Authentication
    }
}