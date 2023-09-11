using DAL.interfaces;
using Domain.Entity;
using Domain.Enum;
using Microsoft.AspNetCore.SignalR;

namespace CodeCup.Hubs
{
    public class UserHub: Hub
    {
        private readonly ILogger<UserHub> _logger;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private List<string> names;
        public UserHub(ILogger<UserHub> logger, IGroupRepository groupRepository, IUserRepository userRepository)
        {
            names = new List<string>() {"Ёжик","Кролик", "Тортик", "Котик", "Булочка", "Пандочка"};
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _logger = logger;
        }
       
        public async Task CreateGroup(string id)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, id);
            await Clients.Group(id).SendAsync("CreateGroup", id);
        }

        public async Task JoinGroupFromLink(string id)
        {
            var group = await _groupRepository.GetAsync(Guid.Parse(id));

            var countUsers = _userRepository.GetAllAsync().Count(x => x.GroupId == Guid.Parse(id));

            if (countUsers < 6)
            {
                var user = new User() {
                Name = names[countUsers],
                DateCreated = DateTime.Now,
                Role = Role.User,
                GroupId = group.Id
                };

                await _userRepository.CreateAsync(user);

                if (group?.Status == Domain.Enum.StatusEntity.Active)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                    await Clients.Group(group.Id.ToString()).SendAsync("UserAdded", user.Name);
                }   
            }
        }
        public async Task SetVote(string groupId, string username, int value)
        {
            var group = await _groupRepository.GetAsync(Guid.Parse(groupId));
            var user = _userRepository.GetAllAsync().Where(x => x.GroupId == Guid.Parse(groupId) 
                && x.Name == username).FirstOrDefault();

            var vote = new Vote() {
                DateCreated = DateTime.Now,
                Value = value,
                UserId = user.Id,
            };
        }
    }
}