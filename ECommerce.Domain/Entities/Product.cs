namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string PictureUrl { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    public Guid ProductBrandId { get; private set; }
    public ProductBrand ProductBrand { get; private set; } = null!;

    public Guid ProductTypeId { get; private set; }
    public ProductType ProductType { get; private set; } = null!;
    private Product() { }

    public Product(
        string name,
        string description,
        string pictureUrl,
        decimal price,
        Guid brandId,
        Guid productTypeId)
    {
        SetName(name);
        SetDescription(description);
        SetPictureUrl(pictureUrl);
        ChangePrice(price);
        ChangeBrand(brandId);
        ChangeProductType(productTypeId);
        ProductBrandId = brandId;
        ProductTypeId = productTypeId;
    }

    public static Product Create(
    string name,
    string description,
    string pictureUrl,
    decimal price,
    Guid brandId,
    Guid productTypeId)
    {
        return new Product(
            name,
            description,
            pictureUrl,
            price,
            brandId,
            productTypeId);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.");

        Name = name.Trim();
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.");

        Description = description.Trim();
    }

    public void SetPictureUrl(string pictureUrl)
    {
        if (string.IsNullOrWhiteSpace(pictureUrl))
            throw new ArgumentException("Picture URL is required.");

        PictureUrl = pictureUrl.Trim();
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new ArgumentException("Price must be greater than zero.");

        Price = newPrice;
    }

    public void ChangeBrand(Guid brandId)
    {
        if (brandId == Guid.Empty)
            throw new ArgumentException("ProductBrand is required.");

        ProductBrandId = brandId;
    }

    public void ChangeProductType(Guid productTypeId)
    {
        if (productTypeId == Guid.Empty)
            throw new ArgumentException("Product type is required.");

        ProductTypeId = productTypeId;
    }

    public void DeleteProduct()
    {
        MarkAsDeleted(Environment.UserName);
    }

    public void RestoreProduct()
    {
        Restore(Environment.UserName);
    }
}