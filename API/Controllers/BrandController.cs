using API.Common;
using Application.Interfaces.Services;
using Application.Models.Common;
using ChauPhatAluminium.Entities;
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
    public async Task<OffsetPage<Brand>> GetBrandPageAsync(int pageNumber = 1, int pageSize = 10, Category? category = null,
        string? name = null)
    {
        return await _brandService.GetBrandPageAsync(pageNumber, pageSize, category, name);
    }
}