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
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly ILogger<GroupService> _logger;
        public GroupService(IUserRepository userRepository, IGroupRepository groupRepository, IUserGroupRepository userGroupRepository, ILogger<GroupService> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<string> CreateAsync(GroupVm model)
        {
            try
            {
                var group = new Group()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    DateCreated = DateTime.Now,
                    Status = StatusEntity.Created
                };

                bool status = await _groupRepository.CreateAsync(group);

                if (status)
                {
                    var user = new User()
                    {
                        Name = model.UserId,
                        Role = Role.Admin,
                        DateCreated = DateTime.Now,
                    };

                    await _userRepository.CreateAsync(user);

                    var userGroup = new UserGroup()
                    {
                        UserId = user.Id,
                        GroupId = group.Id
                    };

                    _logger.LogInformation("При создании UserGroup : " +  userGroup.GroupId + " " + userGroup.UserId);

                    await _userGroupRepository.CreateAsync(userGroup);

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
            try
            {
                var user = new User()
                {
                    Name = model.UserId,
                    Role = Role.User,
                    DateCreated = DateTime.Now,
                };

                await _userRepository.CreateAsync(user);

                var userGroup = new UserGroup()
                {
                    UserId = user.Id,
                    GroupId = Guid.Parse(model.GroupId)
                };

                await _userGroupRepository.CreateAsync(userGroup);

                return user.Id.ToString();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
               _logger.LogError(ex.StackTrace);

                throw;

            }
        }
    }
}