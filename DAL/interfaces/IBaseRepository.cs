namespace DAL.interfaces
{
    public interface IBaseRepository<T>
    {
        public Task<bool> CreateAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> DeleteAsync(T entity);
        public Task<T> GetAsync(int id);
        public IQueryable<T> GetAllAsync();
    }
}