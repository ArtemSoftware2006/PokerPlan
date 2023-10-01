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
        private readonly ILogger<UserHub> _logger;
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly IVotingService _voteService;
        public UserHub(ILogger<UserHub> logger, IGroupService groupService, IUserService userService, IVotingService voteService)
        {
            _voteService = voteService;
            _userService = userService;
            _groupService = groupService;
            _logger = logger;
        }
       
        public async Task CreateGroup(string groupId)
        {
            var response = await _userService.CreateAsync(groupId, Role.Admin);

            if (response.Status == Status.Ok)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, groupId);            

                await Clients.Group(groupId).SendAsync("UserAdded", new List<UserVm>() {response.Data}, StatusEntity.Active);   
            }
        }
        public async Task JoinGroupFromLink(string groupId)
        {
            var responseGroup = await _groupService.GetAsync(groupId);

            if (responseGroup.Status == Status.Ok)
            {
                // TODO возможно для логики создания имени нужен отдельный класс
                var group = responseGroup.Data;
                
                _logger.LogInformation("User: joined group {1}", groupId);

                await _userService.CreateAsync(groupId, Role.User);

                var users = _userService.GetAll().Data.Where(x => x.GroupId == Guid.Parse(groupId)).Select(x => 
                        new UserModel() {Name = x.Name, Id = x.Id, Role = x.Role, isSpectator = Spectator.User })
                        .ToList();

                if (group?.Status != StatusEntity.Closed)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
                    await Clients.Group(groupId).SendAsync("UserAdded", users, group.Status);
                }   
                
            }
            
        }
        public async Task ChooseNameAndSeparator(string groupId, string userId, string username, string isSpectator) 
        {
            //TODO возможно требуется разделить логику изменения имени и логику изменения isSpectator

            Console.WriteLine("User: {0} changed name to {1}", userId, username);

            var userResponse = await _userService.GetAsync(int.Parse(userId));

            if (userResponse.Status == Status.Ok)
            {
                //TODO изменение статуса isSpectator должно быть на уровне сервисов (не уверен, зачем тогда метод Update)
                var user = userResponse.Data;

                user.Name = username;
                user.IsSpectator = (Spectator)int.Parse(isSpectator);

                // TODO не удалять votes, а менять их статус на неактивный
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
        public async Task DeleteVote(string userId)
        {
            _logger.LogInformation("User: {0} deleted vote", userId);

            await _voteService.DeleteByUserIdAsync(int.Parse(userId));   
        }
        public async Task SetVote(VoteVm model)
        {
            var oldVote = _voteService.GetAll().Result.Data
                .Where(x => x.GroupId == Guid.Parse(model.GroupId) && x.UserId == model.UserId)
                .FirstOrDefault();

            if (oldVote != null)
            {
                await _voteService.DeleteAsync(oldVote);
            }

            _logger.LogInformation("User: {0} voted : {1}", model.UserId, model.Value);

            await _voteService.CreateAsync(model);   
        }
        public async Task FinishVoting(string groupId)

        //TODO Рассчет среднего значения должен выполнять отдельный сервис
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
            //TODO Не удалять старые голоса, а менять их статус на неактивные
            var votes = _voteService.GetAll().Result.Data.Where(x => x.GroupId == Guid.Parse(groupId));

            var responseGroup = await _groupService.GetAsync(groupId);

            if (responseGroup.Status == Status.Ok)
            {
                var responseUpdate = await _groupService.ActivateGroupAsync(groupId);

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
                //TODO Закрывать группу в сервисе (может и нет, так как зачем нам метод Update)
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
                //TODO Logout реализовать в сервисе
                User user = response.Data;

                await _userService.DeleteAsync(user);
                var votes = _voteService.GetAll().Result.Data.Where(x => x.UserId == user.Id && x.GroupId == Guid.Parse(groupId));

                await _voteService.DeleteRow(votes.ToList());

                await Clients.Group(groupId).SendAsync("Logout", userId);
            }
        }
    }
}