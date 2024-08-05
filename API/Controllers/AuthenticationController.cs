using API.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("authentication")]
[TypeFilter<ExceptionFilter>]
public class AuthenticationController : Controller
{
    // GET
    [HttpPost]
    public IActionResult Index()
    {
        return View();
    }
}