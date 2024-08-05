using Application.Models.Common;
using Application.Models.Customer;
using ChauPhatAluminium.Entities;

namespace Application.Interfaces.Services;

public interface ICustomerService : IGenericService<Customer>
{
    Task<OffsetPage<CustomerBasicInfo>> GetCustomerPageAsync(int pageNumber, int pageSize, string? phone, string? name);
    Task<CustomerDetailInfo> GetCustomerAsync(int customerId);
}