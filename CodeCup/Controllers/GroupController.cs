using DAL;
using DAL.Impl;
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
        private readonly IUserRepository _userRepository;
        public GroupController(ILogger<GroupController> logger, IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult CreateGroup() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateGroup(string name) 
        {
            string domainName = Request.Host.Value;

            Guid id = Guid.NewGuid();
            string link = domainName  + "/Group/Join/" + id;
            await  _groupRepository.CreateAsync(new Group() 
            {
                DateCreated = DateTime.Now, Id = id, Name = name, Status = StatusEntity.Active
            });

            return View("groupAdmin", new GroupVm() {Name = name, Link = link, Id=id.ToString()});
        }
        [HttpGet("{group}")]
        public async Task<IActionResult> Join(string group)
        {
            try
            {
                string domainName = Request.Host.Value;

                string link = domainName + "/Group/Join/" + group;

                var groupDb = await _groupRepository.GetAsync(Guid.Parse(group));

                var countUsers = _userRepository.GetAllAsync().Count(x => x.GroupId == Guid.Parse(group));

                if (!(countUsers < 6))
                {
                    return Redirect("/Error/NotFound");
                }

                if (groupDb != null && groupDb?.Status == StatusEntity.Active)
                {
                    return View("groupUser", new GroupVm() {Name = groupDb.Name,Id = group, Link = link});
                }
                
                return Redirect("/Error/NotFound");
            }
            catch (System.Exception)
            {
                return Redirect("/Error/NotFound");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}