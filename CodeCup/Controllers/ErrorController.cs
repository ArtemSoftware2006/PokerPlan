using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    public IActionResult NotFound()
    {
        return View();
    }
}
