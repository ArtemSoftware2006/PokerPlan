using DAL.interfaces;
using Domain.Entity;
using Domain.ViewModel;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<bool> CreateAsync(UserVm model)
        {
            try
            {
                var user = new User() 
                {
                    Id = model.UserId,
                    Name = model.Name,
                    GroupId = Guid.Parse(model.GroupId),
                    Role = model.Role,
                    DateCreated = DateTime.Now
                };

                bool status = await _userRepository.CreateAsync(user);

                if(status)
                {
                    _logger.LogInformation("Add user witf id = {0}, name = {1}", user.Id, user.Name);

                    return true;
                }
                
                _logger.LogError("User don`t saved (id = {0}), name = {1})", user.Id, user.Name);

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return false;
            }
        }
    }
}