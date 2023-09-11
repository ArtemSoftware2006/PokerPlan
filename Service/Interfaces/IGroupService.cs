using Domain.Entity;
using Domain.ViewModel;

namespace Service.Interfaces
{
    public interface IGroupService
    {
        public Task<string> CreateAsync(GroupVm model); 
        public Task<string> JoinAsync(Guid groupId, UserVm model);
    }
}