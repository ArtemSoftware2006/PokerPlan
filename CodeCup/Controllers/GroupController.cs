using DAL;
using DAL.interfaces;
using Domain.Entity;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Новая_папка.Models;

namespace Новая_папка.Controllers
{
    [Route("[controller]/[action]")]
    public class GroupController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IGroupRepository _groupRepository;

        public GroupController(ILogger<GroupController> logger, IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
            _logger = logger;
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
        public async Task<IActionResult> CreateGroup(string name) 
        {
            Guid id = Guid.NewGuid();
            string link = "http://localhost:5010/Group/Join/" + id;
            await  _groupRepository.CreateAsync(new Group() 
            {
                DateCreated = DateTime.Now, Id = id, Name = name, Status = StatusEntity.Active
            });

            return View("groupAdmin", new GroupVm() {Name = name, Link = link});
        }
        [HttpGet("{group}")]
        public async Task<IActionResult> Join(string group)
        {
            try
            {
                var groupDb = await _groupRepository.GetAsync(Guid.Parse(group));

                if (groupDb != null && groupDb?.Status == StatusEntity.Active)
                {
                    return View("groupUser", new GroupVm() {Name = groupDb.Name});
                }
                return View("NotFound");
            }
            catch (System.Exception)
            {
                return View("NotFound");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}