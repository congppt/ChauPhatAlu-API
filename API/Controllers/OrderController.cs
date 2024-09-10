using API.Common;
using Application.Interfaces.Services;
using Application.Models.Order;
using ChauPhatAluminium.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("orders")]
[TypeFilter<ExceptionFilter>]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetOrderPageAsync(int pageNumber = 1, int pageSize = 10, OrderStatus? status = null,
        int? customerId = null, DateTime? minDate = null, DateTime? maxDate = null)
    {
        var page = await _orderService.GetOrderPageAsync(pageNumber, pageSize, status, customerId, minDate, maxDate);
        return Ok(page);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderAsync(int id)
    {
        var order = await _orderService.GetOrderAsync(id);
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync(CreateOrder model)
    {
        var guid = await _orderService.CreateOrderAsync(model);
        return Accepted(guid);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateOrderStatusAsync(int id)
    {
        var guid = await _orderService.UpdateOrderStatusAsync(id);
        return Accepted(guid);
    }
}