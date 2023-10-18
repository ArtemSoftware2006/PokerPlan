using DAL.interfaces;
using Domain.Entity;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Impl
{
    public class LinkService : ILinkService
    {
        private string domainName;
        private readonly IGroupRepository _groupRepository;
        private readonly ILogger<LinkService> _logger;

        public LinkService( IGroupRepository groupRepository, ILogger<LinkService> logger,string domainName)
        {
            this.domainName = domainName; 

            _logger = logger;
            _groupRepository = groupRepository;
        }
        public async Task<string> GenerateLink(Guid groupId)
        {
            try
            {
                Group group = await _groupRepository.GetAsync(groupId);

                if (group != null)
                {
                    return domainName + "/Group/JoinToGroup/" + group.Id;
                }

                throw new Exception("Group not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                //TODO Можно ли возращать Null?
                return null; 
            }
        }
    }
}