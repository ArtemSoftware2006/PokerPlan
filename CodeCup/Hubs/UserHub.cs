using Domain.Entity;
using Domain.Enum;
using Domain.ViewModel;
using Microsoft.AspNetCore.SignalR;
using Service.Interfaces;
using Новая_папка.Models;

namespace CodeCup.Hubs
{
    public class UserHub: Hub
    {
        const string NAME_ADMIN = "Администратор";
        private readonly ILogger<UserHub> _logger;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly IVotingService _voteService;
        private List<string> names;
        public UserHub(ILogger<UserHub> logger, IGroupService groupService, IUserService userService, IVotingService voteService)
        {
            names = new List<string>() {"Ёжик","Кролик", "Тортик", "Котик", "Булочка", "Пандочка"};

            _voteService = voteService;
            _userService = userService;
            _groupService = groupService;
            _logger = logger;
        }
       
        public async Task CreateGroup(string groupId)
        {
            var response = await _userService.CreateAsync(new UserVm() {
                Name = NAME_ADMIN,
                Role = Role.Admin,
                GroupId = groupId
            });

            if (response.Status == Status.Ok)
            {
                 await Groups.AddToGroupAsync(Context.ConnectionId, groupId);

                UserModel userVm = new UserModel() 
                {
                    Name = NAME_ADMIN,
                    Id = response.Data.Id
                };

                await Clients.Group(groupId).SendAsync("UserAdded", new List<UserModel>() {userVm});   
            }
        }

        public async Task JoinGroupFromLink(string id)
        {
            var responseGroup = await _groupService.GetAsync(id);

            if (responseGroup.Status == Status.Ok)
            {
                var responseUser = _userService.GetAll();

                if (responseUser.Status == Status.Ok)
                {
                    var userNames = responseUser.Data.Where(x => x.GroupId == Guid.Parse(id)).Select(x => x.Name).ToList();
                    var group = responseGroup.Data;
                    
                    var maybeNames = names.Except(userNames).ToList();

                    _logger.LogInformation("User: {0} joined group {1}", maybeNames[0], id);
                    
                    var user = new UserVm() {
                        Name = maybeNames[0],
                        Role = Role.User,
                        GroupId = group.Id.ToString()
                    };

                    await _userService.CreateAsync(user);

                    var users = _userService.GetAll().Data.Where(x => x.GroupId == Guid.Parse(id)).Select(x => new UserModel() {Name = x.Name, Id = x.Id}).ToList();

                    if (group?.Status != StatusEntity.Closed)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                        await Clients.Group(group.Id.ToString()).SendAsync("UserAdded", users, group.Status);
                    }   
                }
            }
            
        }

        public async Task ChooseNameAndSeparator(string groupId, string userId, string username, string isSpectator) 
        {
            var userResponse = await _userService.GetAsync(int.Parse(userId));

            if (userResponse.Status == Status.Ok)
            {
                var user = userResponse.Data;

                user.Name = username;
                user.IsSpectator = (Spectator)int.Parse(isSpectator);

                if(user.IsSpectator == Spectator.Spectator)  {
                    var votes = _voteService.GetAll().Result.Data.Where(x => x.GroupId == Guid.Parse(groupId) && x.UserId == int.Parse(userId));

                    await _voteService.DeleteRow(votes.ToList());
                }
                    
                

                await _userService.UpdateAsync(user);

                _logger.LogInformation(userId + " changed name to " + username);
                _logger.LogInformation(userId + " choose spectator to " + isSpectator);

                await Clients.Group(groupId).SendAsync("UserChangeName", user.Id, user.Name, user.IsSpectator);      
            }
        }
        public async Task DeleteVote(string groupId, string userId)
        {
            _logger.LogInformation("User: {0} deleted vote", userId);

            Vote vote = _voteService.GetAll().Result.Data.Where(x => x.GroupId == Guid.Parse(groupId) && x.UserId == int.Parse(userId)).FirstOrDefault();

            if (vote != null)
            {
                await _voteService.DeleteAsync(vote);
            }
        }
        public async Task SetVote(CreateVoteModel model)
        {
            var responseGroup = await _groupService.GetAsync(model.GroupId);

            if (responseGroup.Status == Status.Ok)
            {
                var group = responseGroup.Data;

                var user = _userService.GetAll().Data.Where(x => x.GroupId == Guid.Parse(model.GroupId) 
                    && x.Name == model.Username).FirstOrDefault();

                var oldVote = _voteService.GetAll().Result.Data.Where(x => x.GroupId == group.Id && x.UserId == user.Id).FirstOrDefault();

                if (oldVote != null)
                {
                    await _voteService.DeleteAsync(oldVote);
                }

                _logger.LogInformation("User: {0} voted : {1}", user.Name, model.Value);

                var vote = new Vote() {
                    DateCreated = DateTime.Now,
                    GroupId = group.Id,
                    Value = model.Value,
                    UserId = user.Id,
                };

                await _voteService.CreateAsync(vote);   
            }

        }

        public async Task FinishVoting(string groupId)
        {
            float sumValues = 0;
            int countVotind = 0;
            bool isFullConsent = false;
            double average = 0;

            var responseGroup = await _groupService.GetAsync(groupId);

            if (responseGroup.Status == Status.Ok)
            {
                var group = responseGroup.Data;

                var usersInGroup = _userService.GetAll().Data.Where(x => x.GroupId == Guid.Parse(groupId));

                var usersVotes = _voteService.FinishVoting(groupId, usersInGroup.ToList()).Result.Data;

                group.Status = StatusEntity.Stopped;
                var responseUpdate = await _groupService.UpdateAsync(group);

                if (responseUpdate.Status == Status.Ok)
                {
                    foreach (UserVoteVm item in usersVotes)
                    {
                        if (!(item.Value == "?" || item.Value == "Кофе"))
                        {
                            sumValues += float.Parse(item.Value);
                            countVotind++;
                        }
                    }

                    if (countVotind != 0)
                    {
                        isFullConsent = usersVotes.All(x => x.Value == usersVotes[0].Value);

                        average = Math.Round(sumValues / countVotind,1);
                    }
                    else    
                        average = 0;
                    
                    await Clients.Group(groupId).SendAsync("FinishVoting", usersVotes, average, isFullConsent);      
                }
            }
        }
        public async Task StartNewVoting(string groupId) 
        {
            var votes = _voteService.GetAll().Result.Data.Where(x => x.GroupId == Guid.Parse(groupId));

            var responseGroup = await _groupService.GetAsync(groupId);

            if (responseGroup.Status == Status.Ok)
            {
                var group = responseGroup.Data;

                group.Status = StatusEntity.Active;

                var responseUpdate = await _groupService.UpdateAsync(group);

                if(responseUpdate.Status == Status.Ok) {
                    await _voteService.DeleteRow(votes.ToList());

                    await Clients.Group(groupId).SendAsync("StartNewVoting");   
                }
            }

        }
        public async Task CloseGroup(string groupId)
        {
            var responseGroup = await _groupService.GetAsync(groupId);

            if (responseGroup.Status == Status.Ok)
            {
                var group = responseGroup.Data;
                
                group.Status = StatusEntity.Closed;

                var responseUpdate = await _groupService.UpdateAsync(group);

                if (responseUpdate.Status == Status.Ok)
                    await Clients.Group(groupId).SendAsync("CloseGroup");   
            }
        }
        public async Task Logout(string groupId, string userId) 
        {            
            var response = await _userService.GetAsync(int.Parse(userId));

            if (response.Status == Status.Ok)
            {
                User user = response.Data;

                await _userService.DeleteAsync(user);
                var votes = _voteService.GetAll().Result.Data.Where(x => x.UserId == user.Id && x.GroupId == Guid.Parse(groupId));

                await _voteService.DeleteRow(votes.ToList());

                await Clients.Group(groupId).SendAsync("Logout", userId);
            }
        }
    }
}