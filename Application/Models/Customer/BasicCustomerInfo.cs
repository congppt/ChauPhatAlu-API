namespace Application.Models.Customer;

#pragma warning disable CS8618
public class BasicCustomerInfo : MinimalCustomerInfo
{
    public string Phone { get; set; }
    public bool IsMale { get; set; }
    public string Address { get; set; }
}