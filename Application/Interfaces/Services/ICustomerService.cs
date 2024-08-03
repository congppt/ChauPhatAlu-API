using Application.Models.Common;
using ChauPhatAluminium.Entities;

namespace Application.Interfaces.Services;

public interface ICustomerService : IGenericService<Customer>
{
    Task<OffsetPage<Customer>> GetCustomerPageAsync(int pageNumber, int pageSize, string? phone, string? name);
}