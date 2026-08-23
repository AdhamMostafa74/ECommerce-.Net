namespace ECommerce.Application.Common.Files;

public static class ImageFileValidator
{
    public const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

    private static readonly IReadOnlyDictionary<string, byte[]> Signatures =
        new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["image/jpeg"] =
                [0xFF, 0xD8, 0xFF],

            ["image/png"] =
                [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],

            ["image/webp"] =
                [0x52, 0x49, 0x46, 0x46] // RIFF
        };

    public static bool IsValid(FileUpload file)
    {
        if (file.Content.Length > MaxFileSize)
            return false;

        if (!Signatures.TryGetValue(file.ContentType, out var signature))
            return false;

        var stream = file.Content;

        if (!stream.CanSeek)
            return false;

        stream.Position = 0;

        var header = new byte[12];

        var bytesRead = stream.Read(header, 0, header.Length);

        stream.Position = 0;

        if (bytesRead < signature.Length)
            return false;

        // JPEG / PNG
        if (MatchesSignature(header, signature))
            return true;

        // WebP = RIFF....WEBP
        if (file.ContentType.Equals(
                "image/webp",
                StringComparison.OrdinalIgnoreCase))
        {
            return bytesRead >= 12 &&
                   header[8] == 0x57 && // W
                   header[9] == 0x45 && // E
                   header[10] == 0x42 && // B
                   header[11] == 0x50;   // P
        }

        return false;
    }

    private static bool MatchesSignature(
        byte[] header,
        byte[] signature)
    {
        for (var i = 0; i < signature.Length; i++)
        {
            if (header[i] != signature[i])
                return false;
        }

        return true;
    }
}