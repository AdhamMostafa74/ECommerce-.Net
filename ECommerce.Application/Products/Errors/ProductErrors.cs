
using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Types;

namespace ECommerce.Application.Products.Errors
{
    public static class ProductErrors
    {
        // Query Errors
        public static readonly Error NotFound = new(
            "Products.Not Found",
            "The requested product was not found.",
            ErrorType.NotFound);

        // Validation Errors
        public static readonly Error NameRequired = new(
            "Products.Name Required",
            "Product name is required.",
            ErrorType.Validation);

        public static readonly Error DescriptionRequired = new(
            "Products.Description Required",
            "Product description is required.",
             ErrorType.Validation);

        public static readonly Error PictureUrlRequired = new(
            "Products.Picture Url Required",
            "Product picture URL is required.",
             ErrorType.Validation);

        public static readonly Error InvalidPrice = new(
            "Products.Invalid Price",
            "Price must be greater than zero.",
             ErrorType.Validation);

        public static readonly Error BrandRequired = new(
            "Products.Brand Required",
            "A product must belong to a brand.",
             ErrorType.Validation);

        public static readonly Error ProductTypeRequired = new(
            "Products.Product Type Required",
            "A product must belong to a product type.",
             ErrorType.Validation);

        // Business Errors
        public static readonly Error AlreadyDeleted = new(
            "Products.Already Deleted",
            "The product is already deleted.", ErrorType.Failure);

        public static readonly Error ProductAlreadyExists = new(
            "Products.Already Existing",
            "The product is already deleted.", ErrorType.Failure);

        public static readonly Error AlreadyRestored = new(
            "Products.Already Restored",
            "The product is already active.", ErrorType.Failure);
    }
}