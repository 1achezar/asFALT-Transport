using Microsoft.AspNetCore.Mvc;

namespace Transport.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
