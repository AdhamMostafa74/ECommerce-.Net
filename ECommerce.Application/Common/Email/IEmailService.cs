namespace ECommerce.Application.Common.Email;

public interface IEmailService
{
    Task SendEmailAsync(
        string toEmail,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken = default);

    Task SendPasswordResetEmailAsync(
        string toEmail,
        string token,
        CancellationToken cancellationToken = default);

    Task SendEmailChangeConfirmationAsync(
        string toEmail,
        Guid userId,
        string token,
        CancellationToken cancellationToken = default);
}

