using Application.Interfaces.Databases;
using Application.Models.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Customer;

#pragma warning disable CS8618
public class CreateCustomer : Command
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public bool IsMale { get; set; }
    public string Address { get; set; }
}

public class CreateCustomerValidator : AbstractValidator<CreateCustomer>
{
    public CreateCustomerValidator(IAppDbContext dbContext)
    {
        RuleFor(x => x.Name).NotEmpty().Length(3, 20);
        RuleFor(x => x.Phone).MustAsync(async (phone, ct) =>
        {
            return !await dbContext.Customers.AnyAsync(c => c.Phone == phone, ct);
        });
        RuleFor(x => x.Address).NotEmpty().Length(10, 100);
    }
}