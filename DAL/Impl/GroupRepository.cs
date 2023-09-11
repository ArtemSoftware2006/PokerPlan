using DAL.interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Impl
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _dbContext;

        public GroupRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(Group entity)
        {
            await _dbContext.AddAsync(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Group entity)
        {
            _dbContext.Remove(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IQueryable<Group>> GetAllAsync()
        {
            return _dbContext.Groups;
        }

        public async Task<Group> GetAsync(Guid id)
        {
            return await _dbContext.Groups.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(Group entity)
        {
            _dbContext.Update(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}