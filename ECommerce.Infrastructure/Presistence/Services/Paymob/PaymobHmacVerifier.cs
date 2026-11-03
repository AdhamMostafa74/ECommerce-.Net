
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob;

public static class PaymobHmacVerifier
{
    private static readonly string[][] HmacFieldPaths =
    [
        ["amount_cents"],
        ["created_at"],
        ["currency"],
        ["error_occured"],
        ["has_parent_transaction"],
        ["id"],
        ["integration_id"],
        ["is_3d_secure"],
        ["is_auth"],
        ["is_capture"],
        ["is_refunded"],
        ["is_standalone_payment"],
        ["is_voided"],
        ["order", "id"],
        ["owner"],
        ["pending"],
        ["source_data", "pan"],
        ["source_data", "sub_type"],
        ["source_data", "type"],
        ["success"]
    ];

    public static bool VerifyTransaction(
        JsonElement callbackBody,
        string? receivedHmac,
        string secret)
    {
        if (string.IsNullOrWhiteSpace(secret) ||
            string.IsNullOrWhiteSpace(receivedHmac) ||
            receivedHmac.Length != 128)
        {
            return false;
        }

        if (!callbackBody.TryGetProperty("obj", out var transaction) ||
            transaction.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        var values = new StringBuilder();

        foreach (var path in HmacFieldPaths)
        {
            if (!TryGetValue(transaction, path, out var value))
                return false;

            values.Append(value);
        }

        byte[] suppliedSignature;

        try
        {
            suppliedSignature = Convert.FromHexString(receivedHmac);
        }
        catch (FormatException)
        {
            return false;
        }

        if (suppliedSignature.Length != 64)
            return false;

        var keyBytes = Encoding.UTF8.GetBytes(secret);
        var messageBytes = Encoding.UTF8.GetBytes(values.ToString());

        using var hmac = new HMACSHA512(keyBytes);

        var computedSignature = hmac.ComputeHash(messageBytes);

        return CryptographicOperations.FixedTimeEquals(
            computedSignature,
            suppliedSignature);
    }

    private static bool TryGetValue(
        JsonElement element,
        string[] path,
        out string value)
    {
        value = string.Empty;

        foreach (var propertyName in path)
        {
            if (element.ValueKind != JsonValueKind.Object ||
                !element.TryGetProperty(propertyName, out element))
            {
                return false;
            }
        }

        switch (element.ValueKind)
        {
            case JsonValueKind.String:
                value = element.GetString()!;
                return true;

            case JsonValueKind.Number:
            case JsonValueKind.True:
            case JsonValueKind.False:
                value = element.GetRawText();
                return true;

            default:
                return false;
        }
    }
}
