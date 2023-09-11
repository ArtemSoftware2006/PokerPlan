using DAL.interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(User entity)
        {
            await _dbContext.AddAsync(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(User entity)
        {
            _dbContext.Remove(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IQueryable<User>> GetAllAsync()
        {
            return  _dbContext.Users;
        }

        public async Task<User> GetAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            _dbContext.Update(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}