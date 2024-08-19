using API.Common;
using Application.Interfaces.Services;
using ChauPhatAluminium.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("products")]
[TypeFilter<ExceptionFilter>]
public class ProductController : Controller
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductPageAsync(int pageNumber = 1, int pageSize = 10, string? sku = null,
        string? name = null, int? brandId = null, Category? category = null, int minPrice = 0, int? maxPrice = null)
    {
        var page = await _productService.GetProductPageAsync(pageNumber, pageSize, sku, name, brandId, category,
            minPrice, maxPrice);
        return Ok(page);
    }

    // [HttpGet("{id:int}")]
    // public async Task<IActionResult> GetProductAsync(int id)
    // {
    //     var product = await _productService.GetProductAsync(id);
    //     return Ok(product);
    // }
}