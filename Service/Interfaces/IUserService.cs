using Domain.ViewModel;

namespace Service.Interfaces
{
    public interface IUserService
    {
        public Task<bool> CreateAsync(UserVm model); 
    }
}