using DAL.interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL.Impl
{
    public class VoteRepository : IVoteRepository
    {
        private readonly AppDbContext _dbContext;
        public VoteRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(Vote entity)
        {
            await _dbContext.AddAsync(entity);
            
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Vote entity)
        {
            _dbContext.Remove(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IQueryable<Vote> GetAllAsync()
        {
            return _dbContext.Votes;
        }

        public async Task<Vote> GetAsync(int id)
        {
            return await _dbContext.Votes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAsync(Vote entity)
        {
            _dbContext.Update(entity);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}