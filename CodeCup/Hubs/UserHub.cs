using Domain.ViewModel;
using Microsoft.AspNetCore.SignalR;
using Service.Interfaces;

namespace CodeCup.Hubs
{
    public class UserHub: Hub
    {
        private readonly IGroupService _groupService;
        private readonly ILogger<UserHub> _logger;
        public UserHub(IGroupService groupService, ILogger<UserHub> logger)
        {
            _logger = logger;
            _groupService = groupService;
        }
        public async Task SendMessage(string user, string message, string group)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
        }
        public async Task CreateGroup(string name)
        {
            _logger.LogInformation("Id при создании группы: " + Context.ConnectionId);

            string groupName = await _groupService.CreateAsync(new GroupVm() {Name = name, UserId = Context.ConnectionId});

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("CreateGroup", groupName);
        }
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Receive", message);
        }
        public async Task JoinGroupFromLink(string group)
        {
            _logger.LogInformation("Id при присоединении к группе: " + Context.ConnectionId);

            await _groupService.JoinAsync(new UserVm() {UserId = Context.ConnectionId, GroupId = group});

            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await Clients.Group(group).SendAsync("UserAdded", Context.ConnectionId);
        }
    }
}