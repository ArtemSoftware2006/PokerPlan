using DAL.interfaces;
using Domain.Entity;
using Domain.Enum;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Новая_папка.Models;

namespace CodeCup.Hubs
{
    public class UserHub: Hub
    {
        private readonly ILogger<UserHub> _logger;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVoteRepository _voteRepository;
        private List<string> names;
        public UserHub(ILogger<UserHub> logger, IGroupRepository groupRepository, IUserRepository userRepository, IVoteRepository voteRepository)
        {
            names = new List<string>() {"Ёжик","Кролик", "Тортик", "Котик", "Булочка", "Пандочка"};

            _voteRepository = voteRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _logger = logger;
        }
       
        public async Task CreateGroup(string id)
        {
            var user = new User() {
                Name = "Администратор",
                DateCreated = DateTime.Now,
                Role = Role.Admin,
                GroupId = Guid.Parse(id)
            };

            await _userRepository.CreateAsync(user);

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
       
        public async Task SetVote(CreateVoteVm model)
        {
            var group = await _groupRepository.GetAsync(Guid.Parse(model.GroupId));

            var user = _userRepository.GetAllAsync().Where(x => x.GroupId == Guid.Parse(model.GroupId) 
                && x.Name == model.Username).FirstOrDefault();

            var oldVote = _voteRepository.GetAllAsync().Where(x => x.GroupId == group.Id && x.UserId == user.Id).FirstOrDefault();

            if (oldVote != null)
            {
                await _voteRepository.DeleteAsync(oldVote);
            }

            _logger.LogInformation("User: {0} voted : {1}", user.Name, model.Value);

            var vote = new Vote() {
                DateCreated = DateTime.Now,
                GroupId = group.Id,
                Value = int.Parse(model.Value),
                UserId = user.Id,
            };

            await _voteRepository.CreateAsync(vote);

        }

        public async Task FinishVoting(string groupId)
        {
            var usersVotes = (
                from vote in _voteRepository.GetAllAsync().Where(x => x.GroupId == Guid.Parse(groupId))
                join user in _userRepository.GetAllAsync()
                    on  new {UserId = vote.UserId, vote.GroupId} 
                    equals new {UserId = user.Id, user.GroupId}
                select new UsersVote() { Name = user.Name, Value = vote.Value }).ToList();

            double average = usersVotes.Count != 0 ? Math.Round(usersVotes.Average(x => x.Value),1) : 0;
            
            await Clients.Group(groupId).SendAsync("FinishVoting", usersVotes, average);
        }
        
        public async Task StartNewVoting(string groupId) 
        {
            var votes = _voteRepository.GetAllAsync().Where(x => x.GroupId == Guid.Parse(groupId));

            await votes.ForEachAsync(async x => await _voteRepository.DeleteAsync(x));

            await Clients.Group(groupId).SendAsync("StartNewVoting");

        }
        public async Task CloseGroup(string groupId)
        {
            var group = await _groupRepository.GetAsync(Guid.Parse(groupId));
            group.Status = StatusEntity.Closed;

            await _groupRepository.UpdateAsync(group);

            await Clients.Group(groupId).SendAsync("CloseGroup");
        }
    }
}