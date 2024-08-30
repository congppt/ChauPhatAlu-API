using API.Common;
using Application.Interfaces.Services;
using Application.Models.Common;
using Application.Models.Customer;
using MassTransit;
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
    public async Task<IActionResult> GetCustomerPageAsync(int pageNumber = 1, int pageSize = 10, string? phone = null, string? name = null)
    {
        var page = await _customerService.GetCustomerPageAsync(pageNumber, pageSize, phone, name);
        return Ok(page);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustomerAsync(int id)
    {
        var customer = await _customerService.GetCustomerAsync(id);
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomer model)
    {
        var guid = await _customerService.CreateCustomerAsync(model);
        return Accepted(guid);
    }
}