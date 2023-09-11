using DAL.interfaces;
using Domain.Entity;
using Domain.Enum;
using Domain.ViewModel;
using Service.Interfaces;

namespace Service.Impl
{
    public class GroupService : IGroupService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        public GroupService(IUserRepository userRepository, IGroupRepository groupRepository, IUserGroupRepository userGroupRepository)
        {
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
                        Name = Guid.NewGuid().ToString(),
                        Role = Role.Admin,
                        DateCreated = DateTime.Now,
                    };

                    var userGroup = new UserGroup()
                    {
                        UserId = user.Id,
                        GroupId = group.Id
                    };

                    await _userGroupRepository.CreateAsync(userGroup);

                    return group.Id.ToString();
                }

                return null;

            }
            catch (System.Exception ex)
            {
                
                throw;
            }
        }

        public async Task<string> JoinAsync(Guid groupId)
        {
            try
            {
                var user = new User()
                {
                    Name = Guid.NewGuid().ToString(),
                    Role = Role.User,
                    DateCreated = DateTime.Now,
                };

                await _userRepository.CreateAsync(user);

                var userGroup = new UserGroup()
                {
                    UserId = user.Id,
                    GroupId = groupId
                };

                await _userGroupRepository.CreateAsync(userGroup);

                return user.Id.ToString();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }
    }
}