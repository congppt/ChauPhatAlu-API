using ChauPhatAluminium.Enums;
using FluentValidation;

namespace Application.Models.Brand;

#pragma warning disable CS8618
public class CreateBrand
{
    public string Name { get; set; }
}

public class CreateBrandValidator : AbstractValidator<CreateBrand>
{
    public CreateBrandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 10);
    }
}