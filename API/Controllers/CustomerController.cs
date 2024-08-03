using API.Common;
using Application.Interfaces.Services;
using Application.Models.Common;
using ChauPhatAluminium.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("customers")]
[TypeFilter<ExceptionFilter>]
public class CustomerController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<OffsetPage<Customer>> GetCustomerPage(int pageNumber = 1, int pageSize = 10, string? phone = null, string? name = null)
    {
        return await _customerService.GetCustomerPageAsync(pageNumber, pageSize, phone, name);
    }
}