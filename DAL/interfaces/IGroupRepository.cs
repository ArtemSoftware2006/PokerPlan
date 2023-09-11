
using Domain.Entity;

namespace DAL.interfaces
{
    public interface IGroupRepository
    {
         public Task<bool> CreateAsync(Group entity);
        public Task<bool> UpdateAsync(Group entity);
        public Task<bool> DeleteAsync(Group entity);
        public Task<Group> GetAsync(Guid id);
        public IQueryable<Group> GetAllAsync();
    }
}