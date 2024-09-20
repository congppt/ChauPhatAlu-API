using Order.Domain.Abstractions;

namespace Order.Domain.Models;
#pragma warning disable CS8618
public class Customer : Entity
{
    public string Name { get; private set; }
    public string Phone { get; private set; }

    public static Customer Create(int id, string name, string phone)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(phone);
        return new Customer
        {
            Id = id,
            Name = name,
            Phone = phone
        };
    }
}