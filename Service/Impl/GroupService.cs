using System.Runtime.CompilerServices;
using DAL.interfaces;
using Domain.Entity;
using Domain.Enum;
using Domain.ViewModel;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Impl
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly ILogger<GroupService> _logger;
        public GroupService( IGroupRepository groupRepository, ILogger<GroupService> logger)
        {
            _logger = logger;
            _groupRepository = groupRepository;
        }

        public async Task<string> CreateAsync(GroupVm model)
        {
            try
            {
                var group = new Group()
                {
                    Id = model.Id,
                    Name = model.Name,
                    DateCreated = DateTime.Now,
                    Status = StatusEntity.Active
                };

                bool status = await _groupRepository.CreateAsync(group);

                if (status)
                {

                    return group.Id.ToString();
                }

                return null;

            }
            catch (System.Exception ex)
            {
               _logger.LogError(ex.Message);
               _logger.LogError(ex.StackTrace);

               throw;
            }
        }

        public async Task<string> JoinAsync(UserVm model)
        {
            throw  new NotImplementedException();
        }
    }
}