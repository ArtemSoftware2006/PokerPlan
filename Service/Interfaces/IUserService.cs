using Domain;
using Domain.Entity;
using Domain.ViewModel;

namespace Service.Interfaces
{
    public interface IUserService
    {
        public Task<BaseResponse<User>> CreateAsync(UserVm model);  
        public Task<BaseResponse<User>> GetAsync(int id);  
        public Task<BaseResponse<bool>> DeleteAsync(User model);  
        public BaseResponse<List<User>> GetAll();  
    }
}