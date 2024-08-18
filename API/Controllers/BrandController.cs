using API.Common;
using Application.Interfaces.Services;
using Application.Models.Brand;
using ChauPhatAluminium.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("brands")]
[TypeFilter<ExceptionFilter>]
public class BrandController : Controller
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrandPageAsync(int pageNumber = 1, int pageSize = 10, Category? category = null,
        string? name = null)
    {
        var page = await _brandService.GetBrandPageAsync(pageNumber, pageSize, category, name);
        return Ok(page);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBrandAsync(int id)
    {
        var brand = await _brandService.GetBrandAsync(id);
        return Ok(brand);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrandAsync([FromBody] BrandCreate model)
    {
        var topic = await _brandService.CreateBrandAsync(model);
        return Accepted(value: topic);
    }
}