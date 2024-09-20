using Order.Domain.Abstractions;
using Order.Domain.Enums;

namespace Order.Domain.Models;
#pragma warning disable CS8618
public class Product : Entity
{
    public string Name { get; private set; }
    public Category Category { get; private set; }
    public decimal Price { get; private set; }

    public static Product Create(int id, string name, Category category, decimal price)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        return new Product
        {
            Id = id,
            Name = name,
            Category = category,
            Price = price
        };
    }
}