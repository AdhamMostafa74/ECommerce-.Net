using ECommerce.Domain.Common;

namespace ECommerce.Application.Brands.Errors
{
    public static class BrandErrors
    {
        public static readonly Error NotFound = new(
            "Brand.NotFound",
            "The requested brand was not found.",
            ErrorType.NotFound);

        public static readonly Error AlreadyExists = new(
            "Brand.AlreadyExists",
            "A brand with the same name already exists.",
            ErrorType.Conflict);
    }
}