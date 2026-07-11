namespace ECommerce.Domain.Entities;

public class ProductBrand : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<Product> Products { get; private set; } = [];
    private ProductBrand() { }



    public static ProductBrand Create(Guid id, string name)
    {

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Brand name is required.");

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
            throw new ArgumentException("Brand name is required.");

        Name = name.Trim();
    }
}