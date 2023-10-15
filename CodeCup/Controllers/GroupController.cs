using Domain.Cards;
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
        private const int MAX_USERS_IN_GROUP = 6;
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
        public async Task<IActionResult> CreateGroup(string name, int typeCards)
        {
            name = name.Trim();

            string domainName = Request.Host.Value;

            Guid groupId = Guid.NewGuid();
            string link = domainName + "/Group/JoinToGroup/" + groupId;

            //TODO Создание в сервисе
            await _groupService.CreateAsync(new GroupVm()
            {
                Id = groupId,
                Name = name,
                CardSet = (CardSet)typeCards
            });

            return View("groupAdmin",
            new GroupModel()
            {
                Name = name,
                Link = link,
                Id = groupId.ToString(),
                Role = Role.Admin,
                CardsKey = chooceCards((CardSet)typeCards)
            });
        }
        [HttpGet("{groupId}")]
        public async Task<IActionResult> JoinToGroup(string groupId)
        {
            //TODO Реализовать Join в слое сервисов (или проверку на возможность присоединения к группе)
            try
            {
                var response = await _groupService.GetAsync(groupId);

                if (response.Status == Status.Ok)
                {
                    var responseUser = _userService.GetAll();

                    if (responseUser.Status == Status.Ok)
                    {
                        string domainName = Request.Host.Value;

                        string link = domainName + "/Group/JoinToGroup/" + groupId;

                        var countUsers = responseUser.Data.Count(x => x.GroupId == Guid.Parse(groupId));

                        if (!(countUsers < MAX_USERS_IN_GROUP))
                        {
                            return Redirect("/Error/NotFound");
                        }

                        if (response.Data != null && response.Data.Status != StatusEntity.Closed)
                        {
                            return View("groupAdmin",
                                new GroupModel()
                                {
                                    Name = response.Data.Name,
                                    Id = groupId,
                                    Link = link,
                                    Role = Role.User,
                                    CardsKey = chooceCards(response.Data.CardSet)
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

        private KeyValuePair<string, int>[] chooceCards(CardSet cardSet) 
        {
            switch (cardSet)
            {   
                case CardSet.TShirt : return new TShirtCards().getCards();
                case CardSet.Fibonachi : return new FibCards().getCards();
                case CardSet.NaturalNumbers : return new NaturalNumbersCards().getCards();
                
                default: throw new Exception("Набора карточек с номером : " + cardSet);
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