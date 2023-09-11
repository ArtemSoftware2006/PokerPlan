using DAL.interfaces;
using Domain.ViewModel;
using Microsoft.AspNetCore.SignalR;
using Service.Interfaces;

namespace CodeCup.Hubs
{
    public class UserHub: Hub
    {
        private readonly ILogger<UserHub> _logger;
        private readonly IGroupRepository _groupRepository;
        public UserHub(ILogger<UserHub> logger, IGroupRepository groupRepository)
        {
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
            _logger.LogInformation("Join " + Context.ConnectionId);
            var group = await _groupRepository.GetAsync(Guid.Parse(id));

            if (group?.Status == Domain.Enum.StatusEntity.Active)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group.Id.ToString());
                await Clients.Group(group.Id.ToString()).SendAsync("UserAdded", Context.ConnectionId);
            }
        }
    }
}