using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.Application.Authentication.Errors;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCredentials =
        new(
            "Authentication.InvalidCredentials",
        "Invalid email or password.",
        ErrorType.Authentication);

    public static readonly Error RegistrationFailed =
    new(
        "Authentication.RegistrationFailed",
        "Registration failed.",
        ErrorType.Validation);
}