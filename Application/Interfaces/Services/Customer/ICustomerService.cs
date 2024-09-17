using Application.Interfaces.Services.Generic;
using Application.Models.Common;
using Application.Models.Customer;

namespace Application.Interfaces.Services.Customer;

public interface ICustomerService : IGenericService<ChauPhatAluminium.Entities.Customer>
{
    Task<OffsetPage<BasicCustomerInfo>> GetCustomerPageAsync(int pageNumber, int pageSize, string? phone, string? name, CancellationToken ct = default);
    Task<DetailCustomerInfo> GetCustomerAsync(int customerId, CancellationToken ct = default);
    Task<Guid> CreateCustomerAsync(CreateCustomer model, CancellationToken ct = default);
    Task<Guid> UpdateCustomerAsync(UpdateCustomer model, CancellationToken ct = default);
}