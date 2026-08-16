
namespace ECommerce.Infrastructure.Presistence.DataSeeding.Data.Models
{

    public class ProductSeedModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string PictureUrl { get; set; } = string.Empty;

        public Guid BrandId { get; set; }

        public Guid ProductTypeId { get; set; }
    }
}
