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
        private readonly ILinkService _linkService;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;

        public GroupController(ILogger<GroupController> logger, IGroupService groupService, IUserService userService, ILinkService linkService)
        {
            _linkService = linkService;
            _userService = userService;
            _groupService = groupService;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult CreateGroup()
        {
            return View();
        }
        //TODO Возможно CardSet в GroupVm может вызывать ошибки (как превести int к enum на клиенте?)
        [HttpPost]
        public async Task<IActionResult> CreateGroup(GroupVm model)
        {
            try
            {
                model.Name = model.Name.Trim();

                Guid groupId = await _groupService.CreateAsync(model);

                string link = await _linkService.GenerateLink(groupId);

                return View("groupAdmin",
                new GroupModel()
                {
                    Name = model.Name,
                    Link = link,
                    Id = groupId.ToString(),
                    Role = Role.Admin,
                    CardsKey = chooceCards(model.CardSet)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return Redirect("/Error/NotFound");
            }
        }
        [HttpGet("{groupId}")]
        public async Task<IActionResult> JoinToGroup(string groupId)
        {
            //TODO Реализовать Join в слое сервисов (или проверку на возможность присоединения к группе)
            //TODO В NotFound показывать текст ошибки
            try
            {
                var responseGroup = await _groupService.GetAsync(groupId);

                if (responseGroup.Status == Status.Ok)
                {
                    var responseUser = _userService.GetAll();

                    if (responseUser.Status == Status.Ok)
                    {
                        string link = await _linkService.GenerateLink(Guid.Parse(groupId));

                        var countUsers = responseUser.Data.Count(x => x.GroupId == Guid.Parse(groupId));

                        if (!(countUsers < MAX_USERS_IN_GROUP))
                        {
                            return Redirect("/Error/NotFound");
                        }

                        if (responseGroup.Data != null && responseGroup.Data.Status != StatusEntity.Closed)
                        {
                            return View("groupAdmin",
                                new GroupModel()
                                {
                                    Name = responseGroup.Data.Name,
                                    Id = groupId,
                                    Link = link,
                                    Role = Role.User,
                                    CardsKey = chooceCards(responseGroup.Data.CardSet)
                                });
                        }
                    }
                }

                return Redirect("/Error/NotFound");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

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

        public IActionResult ModalHistory(string[] history)
        {
            return View(history);
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