using Application.Interfaces.Databases;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Product;
#pragma warning disable CS8618
public class UpdateProduct
{
    public Guid Guid { get; } = Guid.NewGuid();
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public int WarrantyMonth { get; set; }
    public bool IsAvailable { get; set; }
}

public class UpdateProductValidator : AbstractValidator<UpdateProduct>
{
    public UpdateProductValidator(IAppDbContext dbContext)
    {
        RuleFor(x => x.Id).MustAsync(async (id, ct) =>
        {
            return await dbContext.Products.AnyAsync(p => p.Id == id, ct);
        });
        RuleFor(x => x.Name).NotEmpty().Length(5, 25);
        RuleFor(x => x.Price).InclusiveBetween(1000, 100000000);
        RuleFor(x => x.Description);
        RuleFor(x => x.WarrantyMonth).InclusiveBetween(0, 120);
    }
}