using System.ComponentModel.DataAnnotations.Schema;
using Application.Interfaces.Databases;
using ChauPhatAluminium.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Product;

#pragma warning disable CS8618
public class CreateProduct
{
    public Guid Guid { get; } = Guid.NewGuid();
    public string Name { get; set; }
    public int BrandId { get; set; }
    public string SKU { get; set; }
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public Unit Unit { get; set; }
    public string Description { get; set; }
    public int WarrantyMonth { get; set; }
    public bool IsAvailable { get; set; }
}

public class CreateProductValidator : AbstractValidator<CreateProduct>
{
    private readonly IAppDbContext _context;
    public CreateProductValidator(IAppDbContext context)
    {
        _context = context;
        RuleFor(x => x.Name).NotEmpty().Length(5, 25);
        RuleFor(x => x.BrandId).MustAsync(async (id, ct) =>
        {
            return await _context.Brands.AnyAsync(b => b.Id == id);
        });
        RuleFor(x => x.SKU).NotEmpty().Length(12);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.Price).InclusiveBetween(1000, 100000000);
        RuleFor(x => x.Unit).IsInEnum();
        RuleFor(x => x.Description);
        RuleFor(x => x.WarrantyMonth).InclusiveBetween(0, 120);
    }
}