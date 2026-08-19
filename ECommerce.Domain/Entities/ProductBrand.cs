namespace ECommerce.Domain.Entities;

public class ProductBrand : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    private ProductBrand() { }


    private readonly List<Product> _products = [];
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    public static ProductBrand Create(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("ProductBrand name is required.");

        if (id == Guid.Empty)
            throw new ArgumentException("Please enter an Id", nameof(id));

        return new()
        {
            Id = id,
            Name = name.Trim()
        };
    }
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("ProductBrand name is required.");

        Name = name.Trim();
    }
}