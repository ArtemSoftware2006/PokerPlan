using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Новая_папка.Models;

namespace Новая_папка.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return Redirect("/Group/CreateGroup");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}