using DAL.interfaces;
using Domain;
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

        public async Task<BaseResponse<bool>> ActivateGroupAsync(string groupId)
        {
            try
            {
                var group = await _groupRepository.GetAsync(Guid.Parse(groupId));

                if (group != null)
                {
                    group.Status = StatusEntity.Active;

                    bool status = await _groupRepository.UpdateAsync(group);

                    return new BaseResponse<bool>() {
                        Data = status,
                        Status = Status.Ok
                    };
                }

                throw new Exception("Group not found or error during activation");
            }
            catch (Exception ex)
            {
               _logger.LogError(ex.Message);
               _logger.LogError(ex.StackTrace);

               return new BaseResponse<bool>() {
                    Status = Status.Error,
               };
            }
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
                    Status = StatusEntity.Active,
                    CardSet = model.CardSet
                };

                bool status = await _groupRepository.CreateAsync(group);

                if (status)
                {
                    return group.Id.ToString();
                }

                throw new Exception("Error during creation");

            }
            catch (System.Exception ex)
            {
               _logger.LogError(ex.Message);
               _logger.LogError(ex.StackTrace);

               throw;
            }
        }

        public async Task<BaseResponse<Group>> GetAsync(string groupId)
        {
            try
            {
                var group = await _groupRepository.GetAsync(Guid.Parse(groupId));

                if (group != null) {
                    return new BaseResponse<Group>() {
                        Data = group,
                        Status = Status.Ok
                    };
                }

                throw new Exception("Group not found");
            }
            catch (Exception ex)
            {
               _logger.LogError(ex.Message);
               _logger.LogError(ex.StackTrace);

               return new BaseResponse<Group> {
                    Status = Status.Error
                };
            }
        }

        public async Task<string> JoinAsync(UserVm model)
        {
            throw  new NotImplementedException();
        }

        public async Task<BaseResponse<bool>> StoppedGroupAsync(string groupId)
        {
            try
            {
                var group = await _groupRepository.GetAsync(Guid.Parse(groupId));

                if (group != null)
                {
                    group.Status = StatusEntity.Stopped;

                    bool status = await _groupRepository.UpdateAsync(group);

                    return new BaseResponse<bool>() {
                        Data = status,
                        Status = Status.Ok
                    };
                }

                throw new Exception("Group not found or error during activation");
            }
            catch (Exception ex)
            {
               _logger.LogError(ex.Message);
               _logger.LogError(ex.StackTrace);

               return new BaseResponse<bool>() {
                    Status = Status.Error,
               };
            }
        }

        public async Task<BaseResponse<Group>> UpdateAsync(Group group)
        {
            try
            {
                if (group == null)
                {
                    return new BaseResponse<Group>() {
                        Status = Status.Error
                    };
                }

                bool status = await _groupRepository.UpdateAsync(group);

                if(status) {
                    return new BaseResponse<Group>() {
                        Data = group,
                        Status = Status.Ok
                    };
                }

                return new BaseResponse<Group>() {
                    Status = Status.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

               throw;
            }
        }
    }
}