using Microsoft.AspNetCore.SignalR;

namespace CodeCup.Hubs
{
    public class UserHub: Hub
    {
        public async Task SendMessage(string user, string message, string group)
        {
            await Clients.Group(group).SendAsync("ReceiveMessage", user, message);
            Console.WriteLine(user);
            Console.WriteLine(message);
            Console.WriteLine(group);
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
            Console.WriteLine(group);
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
            await Clients.Group(group).SendAsync("UserAdded", Context.ConnectionId);
        }
    }
}