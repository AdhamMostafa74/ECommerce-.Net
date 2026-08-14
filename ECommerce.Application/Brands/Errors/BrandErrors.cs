using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.Application.Brands.Errors
{
    public static class BrandErrors
    {
        public static readonly Error NotFound = new(
            "ProductBrand.NotFound",
            "The requested brand was not found.",
            ErrorType.NotFound);

        public static readonly Error AlreadyExists = new(
            "ProductBrand.AlreadyExists",
            "A brand with the same name already exists.",
            ErrorType.Conflict);
    }
}