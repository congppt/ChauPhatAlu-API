using Order.Domain.Enums;

namespace Order.Domain.Models;

public class OrderItem
{
    public int OrderId { get; private set; }
    public virtual Order Order { get; private set; }
    public int ProductId { get; private set; }
    public virtual Product Product { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public int Height { get; private set; }
    public int Width { get; private set; }
    public decimal Total { get; private set; }

    internal static OrderItem Create(Product product, int quantity, int height = 0, int width = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
        ArgumentOutOfRangeException.ThrowIfNegative(height);
        ArgumentOutOfRangeException.ThrowIfNegative(width);
        var item = new OrderItem
        {
            Product = product,
            Quantity = quantity,
            Height = height,
            Width = width
        };
        item.Total = item.Price * item.Quantity;
        HashSet<Category> doorCategories = [Category.AluminiumDoor, Category.RollingDoor, Category.SlidingDoor];
        if (!doorCategories.Contains(product.Category)) return item;
        ArgumentOutOfRangeException.ThrowIfZero(height);
        ArgumentOutOfRangeException.ThrowIfZero(width);
        item.Total *= item.Width * item.Height;
        return item;
    }
}