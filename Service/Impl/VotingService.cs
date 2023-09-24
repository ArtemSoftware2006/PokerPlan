using DAL.interfaces;
using Domain;
using Domain.Entity;
using Domain.Enum;
using Domain.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Impl
{
    public class VotingService : IVotingService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly ILogger<VotingService> _logger;

        public VotingService(IVoteRepository voteRepository, ILogger<VotingService> logger)
        {
            _voteRepository = voteRepository;
            _logger = logger;
        }

        public async Task<BaseResponse<bool>> CreateAsync(Vote model)
        {
            try
            {
                bool status = await _voteRepository.CreateAsync(model);

                if (status)
                    return new BaseResponse<bool>() {
                        Data = status,
                        Status = Status.Ok
                    };

                return new BaseResponse<bool>() {
                    Data = status,
                    Status = Status.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<bool> {
                    Status = Status.Error
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Vote model)
        {
            try
            {
                bool status = await _voteRepository.DeleteAsync(model);

                if (status) 
                    return new BaseResponse<bool>() {
                        Data = status,
                        Status = Status.Ok
                    };
                return new BaseResponse<bool>() {
                    Status = Status.Error
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<bool> {
                    Status = Status.Error
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteRow(List<Vote> model)
        {
            try
            {
                bool status = _voteRepository.DeleteRow(model);

                if (status)
                    return new BaseResponse<bool>() {
                        Status = Status.Ok,
                        Data = status
                    };
                
                return new BaseResponse<bool>() {
                    Status = Status.Error
                };
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<bool> {
                    Status = Status.Error
                };
            }
        }

        public async Task<BaseResponse<List<UserVoteVm>>> FinishVoting(string groupId, List<User> users)
        {
            try
            {
                var votes = _voteRepository.GetAllAsync().Where(x => x.GroupId.ToString() == groupId).ToList();

                var usersVotes = from user in users
                    join vote in votes on user.Id equals vote.UserId
                    select new UserVoteVm { Name = user.Name, Value = vote.Value };

                return new BaseResponse<List<UserVoteVm>>() {
                    Data = usersVotes.ToList(),
                    Status = Status.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<List<UserVoteVm>> {
                    Status = Status.Error
                };
            }
        }

        public async Task<BaseResponse<List<Vote>>> GetAll()
        {
            try
            {
                var votes = _voteRepository.GetAllAsync().ToList();

                return new BaseResponse<List<Vote>>() {
                    Data = votes,
                    Status = Status.Ok
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);

                return new BaseResponse<List<Vote>> {
                    Status = Status.Error
                };
            }
        }
    }
}