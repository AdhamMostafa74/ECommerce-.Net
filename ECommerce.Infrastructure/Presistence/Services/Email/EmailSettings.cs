namespace ECommerce.Infrastructure.Presistence.Services.Email;

public sealed class EmailSettings
{
    public const string SectionName = "Email";

    public string SmtpHost { get; init; } = string.Empty;

    public int SmtpPort { get; init; } = 587;

    public string SmtpUsername { get; init; } = string.Empty;

    public string SmtpPassword { get; init; } = string.Empty;

    public string SenderEmail { get; init; } = "noreply@ecommerce.local";

    public string SenderName { get; init; } = "ECommerce Support";

    public bool EnableSsl { get; init; } = true;
}

