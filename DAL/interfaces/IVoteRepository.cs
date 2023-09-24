using Domain.Entity;

namespace DAL.interfaces
{
    public interface IVoteRepository : IBaseRepository<Vote>
    {
        public bool DeleteRow(List<Vote> entities);

    }
}