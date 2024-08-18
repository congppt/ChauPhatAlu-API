namespace Application.Models.Customer;

#pragma warning disable CS8618
public class CustomerCreate
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public bool IsMale { get; set; }
    public string Address { get; set; }
}