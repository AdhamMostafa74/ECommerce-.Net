using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.Application.Account.Errors;

public static class AccountErrors
{
    public static readonly Error ProfileNotFound = new(
        "Account.ProfileNotFound",
        "Customer profile was not found.",
        ErrorType.NotFound);

    public static readonly Error UserNotFound = new(
        "Account.UserNotFound",
        "User account was not found.",
        ErrorType.NotFound);

    public static readonly Error InvalidCurrentPassword = new(
        "Account.InvalidCurrentPassword",
        "The current password is incorrect.",
        ErrorType.Validation);

    public static readonly Error WeakPassword = new(
        "Account.WeakPassword",
        "The password does not meet security requirements.",
        ErrorType.Validation);

    public static readonly Error PasswordChangeFailed = new(
        "Account.PasswordChangeFailed",
        "Unable to change password.",
        ErrorType.Validation);

    public static readonly Error InvalidOrExpiredResetToken = new(
        "Account.InvalidOrExpiredResetToken",
        "Invalid or expired password reset token.",
        ErrorType.Validation);

    public static readonly Error PasswordResetFailed = new(
        "Account.PasswordResetFailed",
        "Unable to reset password.",
        ErrorType.Validation);

    public static readonly Error SameEmail = new(
        "Account.SameEmail",
        "New email address must be different from the current email address.",
        ErrorType.Validation);

    public static readonly Error InvalidOrExpiredEmailChangeToken = new(
        "Account.InvalidOrExpiredEmailChangeToken",
        "Invalid or expired email change confirmation token.",
        ErrorType.Validation);

    public static readonly Error SameUserName = new(
        "Account.SameUserName",
        "New username must be different from the current username.",
        ErrorType.Validation);

    public static readonly Error InvalidUserName = new(
        "Account.InvalidUserName",
        "The specified username format is invalid.",
        ErrorType.Validation);

    public static readonly Error UserNameUpdateFailed = new(
        "Account.UserNameUpdateFailed",
        "Unable to update username.",
        ErrorType.Validation);
}
