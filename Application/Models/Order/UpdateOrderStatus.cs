using FluentValidation;

namespace Application.Models.Order;

public class UpdateOrderStatus
{
    public int Id { get; set; }
    public string Note { get; set; }
}
