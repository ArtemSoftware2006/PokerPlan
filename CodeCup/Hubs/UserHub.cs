using DAL.interfaces;
using Domain.Entity;
using Domain.Enum;
using Microsoft.AspNetCore.SignalR;

namespace CodeCup.Hubs
{
    public class UserHub: Hub
    {
        private readonly IUserRepository userRepository;
        public UserHub(IUserRepository userRepository)
        {
            this.userRepository = userRepository; 
        }
        public async Task SendMessage(string user, string message, string group)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
        }
        public async Task CreateGroup()
        {
            string groupName = Guid.NewGuid().ToString();

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("CreateGroup", groupName);
        }
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync("Receive", message);
        }
        public async Task JoinGroupFromLink(string group)
        {

            await userRepository.CreateAsync(new User() {Name = Context.ConnectionId, Role = Role.User});

            var users = await userRepository.GetAllAsync();

            Console.WriteLine(users.First().Name + " " + users.First().Role);

            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await Clients.Group(group).SendAsync("UserAdded", Context.ConnectionId);
        }
    }
}