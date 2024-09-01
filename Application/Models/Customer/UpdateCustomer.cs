using Application.Interfaces.Databases;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Customer;
#pragma warning disable CS8618
public class UpdateCustomer
{
    public Guid Guid { get; } = Guid.NewGuid();
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public bool IsMale { get; set; }
    public string Address { get; set; }
}

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomer>
{
    public UpdateCustomerValidator(IAppDbContext context)
    {
        RuleFor(x => x.Name).NotEmpty().Length(3, 20);
        RuleFor(x => x.Phone).MustAsync(async (customer, phone, ct) =>
        {
            return !await context.Customers.AnyAsync(c => c.Phone == phone && c.Id != customer.Id, ct);
        });
        RuleFor(x => x.Address).NotEmpty().Length(10, 100);
    }
}