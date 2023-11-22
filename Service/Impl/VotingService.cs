using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using DAL.interfaces;
using Domain;
using Domain.Entity;
using Domain.Enum;
using Domain.ViewModel;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Impl
{
    public class VotingService : IVotingService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly ILogger<VotingService> _logger;
        private readonly IMapper _mapper;

        public VotingService(IVoteRepository voteRepository,
             ILogger<VotingService> logger,
             IMapper mapper)
        {
            _voteRepository = voteRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<BaseResponse<bool>> CreateAsync(VoteVm model)
        {
            try
            {
                var vote = _mapper.Map<Vote>(model);
                vote.DateCreated = DateTime.Now;

                bool status = await _voteRepository.CreateAsync(vote);

                if (!status)
                    throw new Exception("Vote not created");

                return new BaseResponse<bool>() {
                    Data = status,
                    Status = Status.Ok
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

        public async Task<BaseResponse<bool>> DeleteByUserIdAsync(int id)
        {
            try
            {
                var vote = _voteRepository.GetAllAsync().Where(x => x.UserId == id).FirstOrDefault();

                if (vote != null)
                     await _voteRepository.DeleteAsync(vote);
                else
                    throw new Exception("Vote not found");

                return new BaseResponse<bool>() {
                    Status = Status.Ok,
                    Data = true
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
                    select new UserVoteVm { Name = user.Name, Value = vote.Value, Key = vote.Key };

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