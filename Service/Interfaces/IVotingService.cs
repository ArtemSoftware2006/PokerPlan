using Domain;
using Domain.Entity;
using Domain.ViewModel;

namespace Service.Interfaces
{
    public interface IVotingService
    {
        public Task<BaseResponse<List<UserVoteVm>>> FinishVoting(string groupId, List<User> users); 
        public Task<BaseResponse<bool>> CreateAsync(Vote model); 
        public Task<BaseResponse<bool>> DeleteAsync(Vote model); 
        public Task<BaseResponse<bool>> DeleteRow(List<Vote> model); 
        public Task<BaseResponse<List<Vote>>> GetAll(); 
    }
}