using Application.Interfaces.Databases;
using Application.Models.Common;
using ChauPhatAluminium.Enums;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Order;

public class UpdateOrderStatus : Command
{
    public int Id { get; set; }
    public string Note { get; set; }
}

public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatus>
{
    public UpdateOrderStatusValidator(IAppDbContext dbContext)
    {
        RuleFor(x => x.Id).MustAsync(async (id, ct) =>
        {
            return await dbContext.Orders.AnyAsync(o => o.Id == id && o.Status < OrderStatus.Completed, ct);
        });
        
    }
}