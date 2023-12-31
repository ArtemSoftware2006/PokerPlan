using System.Runtime.CompilerServices;
using AutoMapper;
using DAL.interfaces;
using Domain;
using Domain.Entity;
using Domain.Enum;
using Domain.ViewModel;
using Microsoft.Extensions.Logging;
using Service.Helpers;
using Service.Interfaces;

namespace Service.Impl
{
    public class UserService : IUserService
    {
        private UniqueNameGenerator uniqueNameGenerator;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;

            uniqueNameGenerator = new UniqueNameGenerator();
        }
        public async Task<BaseResponse<UserVm>> CreateAsync(string groupId, Role role)
        {
            try
            {
                var user = new User()
                {
                    Name = GenerateNewName(groupId),
                    GroupId = Guid.Parse(groupId),
                    Role = role,
                    DateCreated = DateTime.Now,
                    IsSpectator = RoleInGroup.Participant
                };

                bool status = await _userRepository.CreateAsync(user);

                if (status)
                {
                    _logger.LogInformation("Add user witf id = {0}, name = {1}", user.Id, user.Name);

                    return new BaseResponse<UserVm>
                    {
                        Status = Status.Ok,
                        Data = _mapper.Map<UserVm>(user)
                    };
                }

                throw new Exception("User don`t saved");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<UserVm>
                {
                    Status = Status.Error,
                };
            }
        }
        private string GenerateNewName(string groupId)
        {
            var usernamesInGroup = _userRepository.GetAllAsync()
                                .Where(x => x.GroupId == Guid.Parse(groupId))
                                .Select(x => x.Name)
                                .ToList();

            return uniqueNameGenerator.GenerateNewName(usernamesInGroup);
        }

        public async Task<BaseResponse<bool>> DeleteAsync(User model)
        {
            try
            {
                bool status = await _userRepository.DeleteAsync(model);

                if (status)
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        Status = Status.Ok
                    };

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Status = Status.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<bool>
                {
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

                if (users != null)
                {
                    return new BaseResponse<List<User>>()
                    {
                        Data = users,
                        Status = Status.Ok
                    };
                }

                return new BaseResponse<List<User>>
                {
                    Status = Status.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<List<User>>
                {
                    Status = Status.Error,
                };
            }
        }

        public async Task<BaseResponse<User>> GetAsync(int id)
        {
            try
            {
                var user = await _userRepository.GetAsync(id);

                if (user != null)
                    return new BaseResponse<User>()
                    {
                        Data = user,
                        Status = Status.Ok
                    };

                return new BaseResponse<User>()
                {
                    Status = Status.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<User>
                {
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
                    return new BaseResponse<bool>()
                    {
                        Data = true,
                        Status = Status.Ok
                    };

                return new BaseResponse<bool>()
                {
                    Status = Status.Error,
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<bool>()
                {
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
                        return new BaseResponse<bool>()
                        {
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

                return new BaseResponse<bool>()
                {
                    Data = false,
                    Status = Status.Error
                };
            }
        }
    }
}