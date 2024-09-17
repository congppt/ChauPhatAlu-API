using Application.Interfaces.Databases;
using Application.Models.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Brand;

#pragma warning disable CS8618
public class CreateBrand : Command
{
    public string Name { get; set; }
}

public class CreateBrandValidator : AbstractValidator<CreateBrand>
{
    public CreateBrandValidator(IAppDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 10).MustAsync(async (name, ct) =>
        {
            return !await dbContext.Brands.AnyAsync(b => b.Name == name, ct);
        });
    }
}