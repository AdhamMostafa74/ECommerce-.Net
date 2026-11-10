using System.Net;
using System.Net.Mail;
using ECommerce.Application.Common.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Presistence.Services.Email;

public sealed class SmtpEmailService(
    IOptions<EmailSettings> options,
    ILogger<SmtpEmailService> logger)
    : IEmailService
{
    private readonly EmailSettings _settings = options.Value;
    private readonly ILogger<SmtpEmailService> _logger = logger;

    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string htmlBody,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_settings.SmtpHost) ||
            string.IsNullOrWhiteSpace(_settings.SmtpUsername) ||
            string.IsNullOrWhiteSpace(_settings.SmtpPassword))
        {
            _logger.LogInformation(
                "SMTP credentials are not configured. Skipping external email dispatch for subject '{Subject}' to {Recipient}.",
                subject,
                toEmail);
            return;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(toEmail));

        using var client = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
        {
            EnableSsl = _settings.EnableSsl,
            Credentials = new NetworkCredential(
                _settings.SmtpUsername,
                _settings.SmtpPassword)
        };

        await client.SendMailAsync(message, cancellationToken);

        _logger.LogInformation(
            "Transactional email '{Subject}' sent to {Recipient}.",
            subject,
            toEmail);
    }

    public Task SendPasswordResetEmailAsync(
        string toEmail,
        string token,
        CancellationToken cancellationToken = default)
    {
        var encodedOtp = WebUtility.HtmlEncode(token);

        var htmlBody = $"""
            <div style="font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto;">
                <h2>Password Reset Verification Code</h2>
                <p>Hello,</p>
                <p>Use the following 6-digit verification code to reset your ECommerce account password:</p>
                <div style="font-size: 28px; font-weight: bold; letter-spacing: 6px; padding: 14px 20px; background-color: #f4f6f8; border-radius: 6px; text-align: center; margin: 20px 0;">
                    {encodedOtp}
                </div>
                <p>This code is valid for a limited time and can only be used once.</p>
                <p>If you did not request a password reset, you can safely ignore this email.</p>
            </div>
            """;

        return SendEmailAsync(
            toEmail,
            "Your ECommerce Password Reset Code",
            htmlBody,
            cancellationToken);
    }

    public Task SendEmailChangeConfirmationAsync(
        string toEmail,
        Guid userId,
        string token,
        CancellationToken cancellationToken = default)
    {
        var encodedOtp = WebUtility.HtmlEncode(token);
        var encodedUserId = WebUtility.HtmlEncode(userId.ToString());

        var htmlBody = $"""
            <div style="font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto;">
                <h2>Confirm Your New Email Address</h2>
                <p>Hello,</p>
                <p>Use the following 6-digit verification code to confirm your new ECommerce email address:</p>
                <div style="font-size: 28px; font-weight: bold; letter-spacing: 6px; padding: 14px 20px; background-color: #f4f6f8; border-radius: 6px; text-align: center; margin: 20px 0;">
                    {encodedOtp}
                </div>
                <p style="font-size: 12px; color: #666;">Account User ID: <code>{encodedUserId}</code></p>
                <p>This code is valid for a limited time and can only be used once.</p>
                <p>If you did not request this change, please contact support immediately.</p>
            </div>
            """;

        return SendEmailAsync(
            toEmail,
            "Your ECommerce Email Change Verification Code",
            htmlBody,
            cancellationToken);
    }
}
