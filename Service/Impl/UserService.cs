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
        //TODO Убрать костыль с выбором имен
        private List<string> names = new List<string>() {"Ёжик","Кролик", "Тортик", "Котик", "Булочка", "Пандочка"};
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<BaseResponse<UserVm>> CreateAsync(string groupId, Role role)
        {
            try
            {
                var user = new User() 
                {
                    //TODO Убрать костыль с выбором имен
                    Name = names[new Random().Next(0, names.Count)],
                    GroupId = Guid.Parse(groupId),
                    Role = role,
                    DateCreated = DateTime.Now,
                    IsSpectator = Spectator.User
                };

                bool status = await _userRepository.CreateAsync(user);

                if(status)
                {
                    _logger.LogInformation("Add user witf id = {0}, name = {1}", user.Id, user.Name);

                    return new BaseResponse<UserVm> {
                        Status = Status.Ok,
                        Data = new UserVm() {
                            Name = user.Name,
                            Role = user.Role,
                            IsSpectator = user.IsSpectator,
                            Id = user.Id
                        }
                    };
                }

                _logger.LogError("User don`t saved (id = {0}), name = {1})", user.Id, user.Name);

                return new BaseResponse<UserVm> {
                    Status = Status.Error,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<UserVm> {
                    Status = Status.Error,
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

        public async Task<BaseResponse<bool>> UpdateAsync(User model)
        {
            try
            {
                bool status = await _userRepository.UpdateAsync(model);

                if (status)
                    return new BaseResponse<bool>() {
                        Data = true,
                        Status = Status.Ok
                    };
                
                return new BaseResponse<bool>() {
                    Status = Status.Error,
                };
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<bool>() {
                    Data = false,
                    Status = Status.Error
                }; 
            }
        }

        public async Task<BaseResponse<bool>> Logout(int userId)
        {
            try
            {
                var user = await _userRepository.GetAsync(userId);

                if (user != null)
                {
                    bool status = await _userRepository.DeleteAsync(user);

                    if (status)
                    return new BaseResponse<bool>() {
                        Data = true,
                        Status = Status.Ok
                    };
                }
                
                throw new Exception("User don`t logout");
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<bool>() {
                    Data = false,
                    Status = Status.Error
                }; 
            }
        }
    }
}