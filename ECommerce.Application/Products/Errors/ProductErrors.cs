using ECommerce.Domain.Common;

namespace ECommerce.Application.Products.Errors
{
    public static class ProductErrors
    {
        // Query Errors
        public static readonly Error NotFound = new(
            "Products.NotFound",
            "The requested product was not found.");

        // Validation Errors
        public static readonly Error NameRequired = new(
            "Products.NameRequired",
            "Product name is required.");

        public static readonly Error DescriptionRequired = new(
            "Products.DescriptionRequired",
            "Product description is required.");

        public static readonly Error PictureUrlRequired = new(
            "Products.PictureUrlRequired",
            "Product picture URL is required.");

        public static readonly Error InvalidPrice = new(
            "Products.InvalidPrice",
            "Price must be greater than zero.");

        public static readonly Error BrandRequired = new(
            "Products.BrandRequired",
            "A product must belong to a brand.");

        public static readonly Error ProductTypeRequired = new(
            "Products.ProductTypeRequired",
            "A product must belong to a product type.");

        // Business Errors
        public static readonly Error AlreadyDeleted = new(
            "Products.AlreadyDeleted",
            "The product is already deleted.");

        public static readonly Error AlreadyRestored = new(
            "Products.AlreadyRestored",
            "The product is already active.");
    }
}