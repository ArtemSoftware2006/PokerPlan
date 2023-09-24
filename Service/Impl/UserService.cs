using System.Runtime.CompilerServices;
using DAL.interfaces;
using Domain;
using Domain.Entity;
using Domain.Enum;
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
        public async Task<BaseResponse<User>> CreateAsync(UserVm model)
        {
            try
            {
                var user = new User() 
                {
                    Name = model.Name,
                    GroupId = Guid.Parse(model.GroupId),
                    Role = model.Role,
                    DateCreated = DateTime.Now
                };

                bool status = await _userRepository.CreateAsync(user);

                if(status)
                {
                    _logger.LogInformation("Add user witf id = {0}, name = {1}", user.Id, user.Name);

                    return new BaseResponse<User> {
                        Status = Domain.Enum.Status.Ok,
                        Data = user
                    };
                }

                _logger.LogError("User don`t saved (id = {0}), name = {1})", user.Id, user.Name);

                return new BaseResponse<User> {
                    Status = Domain.Enum.Status.Error,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<User> {
                    Status = Status.Error,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteAsync(User model)
        {
            try
            {
                bool status = await _userRepository.DeleteAsync(model);

                if (status)
                    return new BaseResponse<bool>() {
                        Data = true,
                        Status = Status.Ok
                    };
                    
                return new BaseResponse<bool>() {
                    Data = false,
                    Status = Status.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<bool> {
                    Status = Status.Error,
                    Data = false
                };
            }
        }

        public BaseResponse<List<User>> GetAll()
        {
            try
            {
                var users = _userRepository.GetAllAsync().ToList();

                if(users != null) {
                    return new BaseResponse<List<User>>() {
                        Data = users,
                        Status = Domain.Enum.Status.Ok
                    };
                }

                return new BaseResponse<List<User>> {
                    Status = Domain.Enum.Status.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<List<User>> {
                    Status = Domain.Enum.Status.Error,
                };
            }
        }

        public async Task<BaseResponse<User>> GetAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetAsync(id);

                if (user != null)
                    return new BaseResponse<User>() {
                        Data = user,
                        Status = Status.Ok
                    };
                
                return new BaseResponse<User>() {
                    Status = Status.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<User> {
                    Status = Status.Error,
                };
            }
        }
    }
}