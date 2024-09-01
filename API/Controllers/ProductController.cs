using API.Common;
using Application.Interfaces.Services;
using Application.Models.Common;
using Application.Models.Product;
using ChauPhatAluminium.Enums;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("products")]
[TypeFilter<ExceptionFilter>]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly IPublishEndpoint _publisher;

    public ProductController(IProductService productService, IPublishEndpoint publisher)
    {
        _productService = productService;
        _publisher = publisher;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductPageAsync(int pageNumber = 1, int pageSize = 10, string? sku = null,
        string? name = null, int? brandId = null, Category? category = null, int minPrice = 0, int? maxPrice = null)
    {
        var page = await _productService.GetProductPageAsync(pageNumber, pageSize, sku, name, brandId, category,
            minPrice, maxPrice);
        return Ok(page);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductAsync(int id)
    {
        var product = await _productService.GetProductAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync(CreateProduct model)
    {
        var guid = await _productService.CreateProductAsync(model);
        return Accepted(guid);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateProductAsync(UpdateProduct model)
    {
        var guid = await _productService.UpdateProductAsync(model);
        return Accepted(guid);
    }
}