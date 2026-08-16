using ECommerce.Domain.Common.Types;

namespace ECommerce.Domain.Common
{
    public sealed record Error(
        string Code,
        string Description,
        ErrorType Type)
    {
        public static readonly Error None =
            new(string.Empty, string.Empty, ErrorType.None);
    }
}