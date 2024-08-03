using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthenticationController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}