using ECommerce.API.Extensions;
using ECommerce.API.Responses;
using ECommerce.Application.Account.Commands.ChangeEmail;
using ECommerce.Application.Account.Commands.ChangePassword;
using ECommerce.Application.Account.Commands.ConfirmEmailChange;
using ECommerce.Application.Account.Commands.ForgotPassword;
using ECommerce.Application.Account.Commands.ResetPassword;
using ECommerce.Application.Account.Commands.UpdateProfile;
using ECommerce.Application.Account.Commands.UpdateUserName;
using ECommerce.Application.Account.Queries.GetProfile;
using MediatR;

namespace ECommerce.API.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/account")
            .WithTags("Account");

        // Retrieve Current Profile
        group.MapGet("/me", async (
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await sender.Send(
                new GetProfileQuery(),
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization()
        .WithName("GetCurrentProfile")
        .WithSummary("Get current customer profile")
        .WithDescription(
            "Retrieves the authenticated customer's personal profile and account credentials.")
        .Produces<ApiResponse<ProfileResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<ProfileResponse>>(
            StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<ProfileResponse>>(
            StatusCodes.Status404NotFound);

        // Update Current Profile
        group.MapPut("/me", async (
            UpdateProfileCommand command,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await sender.Send(
                command,
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization()
        .WithName("UpdateCurrentProfile")
        .WithSummary("Update current customer profile")
        .WithDescription(
            "Updates the authenticated customer's personal profile information without modifying authentication credentials.")
        .Produces<ApiResponse<ProfileResponse>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<ProfileResponse>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<ProfileResponse>>(
            StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<ProfileResponse>>(
            StatusCodes.Status404NotFound);

        // Change Password
        group.MapPost("/change-password", async (
            ChangePasswordCommand command,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await sender.Send(
                command,
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization()
        .WithName("ChangePassword")
        .WithSummary("Change current user password")
        .WithDescription(
            "Changes the authenticated user's password after verifying the current password.")
        .Produces<ApiResponse<string>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status401Unauthorized);

        // Request Password Reset
        group.MapPost("/forgot-password", async (
            ForgotPasswordCommand command,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await sender.Send(
                command,
                ct);

            return result.ToApiResult(context);
        })
        .AllowAnonymous()
        .RequireRateLimiting("SensitiveAnonymous")
        .WithName("ForgotPassword")
        .WithSummary("Request a password reset link")
        .WithDescription(
            "Generates a password reset token and sends a secure reset link to the specified email address if registered.")
        .Produces<ApiResponse<string>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(
            StatusCodes.Status429TooManyRequests);

        // Reset Password
        group.MapPost("/reset-password", async (
            ResetPasswordCommand command,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await sender.Send(
                command,
                ct);

            return result.ToApiResult(context);
        })
        .AllowAnonymous()
        .RequireRateLimiting("SensitiveAnonymous")
        .WithName("ResetPassword")
        .WithSummary("Reset password using recovery token")
        .WithDescription(
            "Resets the user's password using a valid password reset token.")
        .Produces<ApiResponse<string>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<object>>(
            StatusCodes.Status429TooManyRequests);

        // Change Email
        group.MapPost("/change-email", async (
            ChangeEmailCommand command,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await sender.Send(
                command,
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization()
        .WithName("ChangeEmail")
        .WithSummary("Request an email address change")
        .WithDescription(
            "Validates availability of the new email address and sends a confirmation link to the new email without immediately replacing the current email.")
        .Produces<ApiResponse<string>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status409Conflict);

        // Confirm Email Change
        group.MapPost("/confirm-email-change", async (
            ConfirmEmailChangeCommand command,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await sender.Send(
                command,
                ct);

            return result.ToApiResult(context);
        })
        .AllowAnonymous()
        .RequireRateLimiting("SensitiveAnonymous")
        .WithName("ConfirmEmailChange")
        .WithSummary("Confirm email address change")
        .WithDescription(
            "Validates the email change confirmation token and completes the email address update.")
        .Produces<ApiResponse<string>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status409Conflict)
        .Produces<ApiResponse<object>>(
            StatusCodes.Status429TooManyRequests);

        // Update Username
        group.MapPut("/username", async (
            UpdateUserNameCommand command,
            ISender sender,
            HttpContext context,
            CancellationToken ct) =>
        {
            var result = await sender.Send(
                command,
                ct);

            return result.ToApiResult(context);
        })
        .RequireAuthorization()
        .WithName("UpdateUserName")
        .WithSummary("Update current user's username")
        .WithDescription(
            "Validates format and uniqueness of the requested username and updates it for the authenticated user.")
        .Produces<ApiResponse<string>>(
            StatusCodes.Status200OK)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status401Unauthorized)
        .Produces<ApiResponse<string>>(
            StatusCodes.Status409Conflict);

        return endpoints;
    }
}
