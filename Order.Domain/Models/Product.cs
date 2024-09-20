using Order.Domain.Abstractions;

namespace Order.Domain.Models;
#pragma warning disable CS8618
public class Product : Entity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public static Product Create(int id, string name, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return new Product()
        {
            Id = id,
            Name = name,
            Price = price
        };
    }
}