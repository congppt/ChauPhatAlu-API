using API.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("orders")]
[TypeFilter<ExceptionFilter>]
public class OrderController : Controller
{
    // GET
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}