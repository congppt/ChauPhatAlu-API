using Application.Interfaces.Databases;
using ChauPhatAluminium.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Order;
#pragma warning disable CS8618
public class CreateOrder
{
    public int CustomerId { get; set; }
    public string Address { get; set; }
    public Dictionary<int, ProductOption> Products { get; set; }

    public class ProductOption
    {
        public int Quantity { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}

public class CreateOrderValidator : AbstractValidator<CreateOrder>
{
    public CreateOrderValidator(IAppDbContext context)
    {
        RuleFor(x => x.CustomerId).MustAsync(async (id, ct) =>
        {
            return await context.Customers.AnyAsync(c => c.Id == id, ct);
        });
        RuleFor(x => x.Address).NotEmpty().Length(10, 100);
        RuleFor(x => x.Products).Cascade(CascadeMode.Stop).CustomAsync(async (items, ctx, ct) =>
        {
            var products = await context.Products.Where(p => items.Keys.Contains(p.Id) && p.IsAvailable)
                .ToListAsync(ct);
            if (!IsAllProductsFound(products, items))
            {
                ctx.AddFailure("");
            }

            foreach (var product in products.Where(product => !IsProductOptionValid(product, items[product.Id])))
                ctx.AddFailure("");
            
        });
    }

    private bool IsAllProductsFound(IList<ChauPhatAluminium.Entities.Product> found,
        IDictionary<int, CreateOrder.ProductOption> origin) => found.Count == origin.Count;

    private bool IsProductOptionValid(ChauPhatAluminium.Entities.Product product, CreateOrder.ProductOption option)
    {
        var isValid = option.Quantity >= 1;
        switch (product.Category)
        {
            case Category.RollingDoor:
            case Category.AluminiumDoor:
            case Category.SlidingDoor:
                isValid = isValid && option.Height is >= 1 and <= 100;
                isValid = isValid && option.Width is >= 1 and <= 100;
                break;
        }
        return isValid;
    }
}