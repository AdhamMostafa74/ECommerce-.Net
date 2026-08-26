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

    public static readonly Error UsernameAlreadyExists =
        new(
            "Authentication.UsernameAlreadyExists",
            "Username is already taken.",
            ErrorType.Conflict);

    public static readonly Error EmailAlreadyExists =
        new(
            "Authentication.EmailAlreadyExists",
            "Email is already registered.",
            ErrorType.Conflict);

    public static readonly Error InvalidRegistrationData =
        new(
            "Authentication.InvalidRegistrationData",
            "The registration data is invalid.",
            ErrorType.Validation);

    public static readonly Error RegistrationFailed =
        new(
            "Authentication.RegistrationFailed",
            "Registration failed.",
            ErrorType.Failure);
}