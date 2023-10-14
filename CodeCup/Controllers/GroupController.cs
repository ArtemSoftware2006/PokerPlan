using Domain.Enum;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Новая_папка.Models;

namespace Новая_папка.Controllers
{
    [Route("[controller]/[action]")]
    public class GroupController : Controller
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        public GroupController(ILogger<GroupController> logger, IGroupService groupService, IUserService userService)
        {
            _userService = userService;
            _groupService = groupService;
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
            name = name.Trim();

            string domainName = Request.Host.Value;

            Guid groupId = Guid.NewGuid();
            string link = domainName + "/Group/JoinToGroup/" + groupId;

            await _groupService.CreateAsync(new GroupVm()
            {
                Id = groupId,
                Name = name
            });

            return View("groupAdmin",
            new GroupModel()
            {
                Name = name,
                Link = link,
                Id = groupId.ToString(),
                Role = Role.Admin,
            });
        }
        [HttpGet("{group}")]
        public async Task<IActionResult> JoinToGroup(string group)
        {
            //TODO Реализовать Join в слое сервисов (или проверку на возможность присоединения к группе)
            try
            {
                var response = await _groupService.GetAsync(group);

                if (response.Status == Status.Ok)
                {
                    var responseUser = _userService.GetAll();

                    if (response.Status == Status.Ok)
                    {
                        string domainName = Request.Host.Value;

                        string link = domainName + "/Group/Join/" + group;

                        var countUsers = responseUser.Data.Count(x => x.GroupId == Guid.Parse(group));

                        if (!(countUsers < 6))
                        {
                            return Redirect("/Error/NotFound");
                        }

                        if (response.Data != null && response.Data.Status != StatusEntity.Closed)
                        {
                            //TODO В GroupMidel должна быть добавлена роль пользователя
                            return View("groupAdmin",
                                new GroupModel()
                                {
                                    Name = response.Data.Name,
                                    Id = group,
                                    Link = link,
                                    Role = Role.User,
                                });
                        }
                    }
                }

                return Redirect("/Error/NotFound");
            }
            catch (System.Exception)
            {
                return Redirect("/Error/NotFound");
            }

        }

        public IActionResult ModalLink(string link)
        {
            return View(link);
        }

        public IActionResult ModalChooseNameAndSpectator(ChooseNameAndSpectatorModel model)
        {
            return View(model);
        }

        public IActionResult Header(string username)
        {
            return View(username);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}