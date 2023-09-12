using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    public IActionResult NotFound()
    {
        return View();
    }
    public IActionResult GroupClosed()
    {
        return View();
    }
}
