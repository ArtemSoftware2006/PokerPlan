using DAL.interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Impl
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly AppDbContext _dbContext;
        public UserGroupRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(UserGroup entity)
        {
            await _dbContext.AddAsync(entity);
            
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(UserGroup entity)
        {
            _dbContext.Remove(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IQueryable<UserGroup> GetAllAsync()
        {
            return  _dbContext.UserGroups;
        }

        public async Task<UserGroup> GetAsync(int id)
        {
            return await _dbContext.UserGroups.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(UserGroup entity)
        {
            _dbContext.Update(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}