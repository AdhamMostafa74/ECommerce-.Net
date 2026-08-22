namespace ECommerce.Application.Common.Files;

public sealed record FileUpload(
    Stream Content,
    string FileName,
    string ContentType);