
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ECommerce.Infrastructure.Presistence.Services.Paymob;

public static class PaymobHmacVerifier
{
    private static readonly string[] HmacFields =
    [
        "amount_cents",
        "created_at",
        "currency",
        "error_occured",
        "has_parent_transaction",
        "id",
        "integration_id",
        "is_3d_secure",
        "is_auth",
        "is_capture",
        "is_refunded",
        "is_standalone_payment",
        "is_voided",
        "order.id",
        "owner",
        "pending",
        "source_data.pan",
        "source_data.sub_type",
        "source_data.type",
        "success"
    ];

    public static bool VerifyTransaction(
        JsonElement root,
        string suppliedHmac,
        string secret)
    {
        if (string.IsNullOrWhiteSpace(suppliedHmac) ||
            string.IsNullOrWhiteSpace(secret) ||
            !root.TryGetProperty("obj", out var transaction))
        {
            return false;
        }

        var values = new StringBuilder();

        foreach (var field in HmacFields)
        {
            if (!TryGetFieldValue(transaction, field, out var value))
                return false;

            values.Append(value);
        }

        using var hmac = new HMACSHA512(
            Encoding.UTF8.GetBytes(secret));

        var computedHash = hmac.ComputeHash(
            Encoding.UTF8.GetBytes(values.ToString()));

        byte[] suppliedHash;

        try
        {
            suppliedHash = Convert.FromHexString(suppliedHmac);
        }
        catch (FormatException)
        {
            return false;
        }

        return suppliedHash.Length == computedHash.Length &&
               CryptographicOperations.FixedTimeEquals(
                   computedHash,
                   suppliedHash);
    }

    private static bool TryGetFieldValue(
        JsonElement transaction,
        string field,
        out string value)
    {
        value = string.Empty;

        var path = field.Split('.');
        var current = transaction;

        foreach (var segment in path)
        {
            if (current.ValueKind != JsonValueKind.Object ||
                !current.TryGetProperty(segment, out var child))
            {
                return false;
            }

            current = child;
        }

        switch (current.ValueKind)
        {
            case JsonValueKind.String:
                value = current.GetString() ?? string.Empty;
                return true;

            case JsonValueKind.Number:
                value = current.GetRawText();
                return true;

            case JsonValueKind.True:
                value = "true";
                return true;

            case JsonValueKind.False:
                value = "false";
                return true;

            case JsonValueKind.Null:
                value = string.Empty;
                return true;

            default:
                return false;
        }
    }
}
