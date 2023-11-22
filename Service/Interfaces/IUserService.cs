using Domain;
using Domain.Entity;
using Domain.Enum;
using Domain.ViewModel;

namespace Service.Interfaces
{
    public interface IUserService
    {
        public Task<BaseResponse<UserVm>> CreateAsync(string groupId, Role role);  
        public Task<BaseResponse<User>> GetAsync(int id);  
        public Task<BaseResponse<bool>> DeleteAsync(User model);  
        public Task<BaseResponse<bool>> UpdateAsync(User model);  
        public Task<BaseResponse<bool>> Logout(int userId);  
        public BaseResponse<List<User>> GetAll();  

    }
}