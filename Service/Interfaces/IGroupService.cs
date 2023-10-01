using Domain;
using Domain.Entity;
using Domain.ViewModel;

namespace Service.Interfaces
{
    public interface IGroupService
    {
        public Task<string> CreateAsync(GroupVm model); 
        public Task<string> JoinAsync(UserVm groupId);
        public Task<BaseResponse<Group>> GetAsync(string groupId);
        public Task<BaseResponse<Group>> UpdateAsync(Group group);
        public Task<BaseResponse<bool>> ActivateGroupAsync(string groupId);
        public Task<BaseResponse<bool>> CloseGroupAsync(string groupId);
    }
}