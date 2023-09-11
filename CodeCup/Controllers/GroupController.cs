using DAL;
using Microsoft.AspNetCore.Mvc;
using Новая_папка.Models;

namespace Новая_папка.Controllers
{
    [Route("[controller]/[action]")]
    public class GroupController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly AppDbContext _dbContext ;

        public GroupController(ILogger<GroupController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Route("{group}")]
        public IActionResult CreateLinkGroup(Guid group)
        {
            _logger.LogInformation(group.ToString());
            return View("CreateLinkGroup", new GroupModel() {Id = group});
        }
        [HttpGet]
        public IActionResult CreateGroup() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateGroup(string name) 
        {
            string link = "http://localhost:5010/Group/Join/" + Guid.NewGuid();

            return View("groupAdmin", new GroupVm() {Name = name, Link = link});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}