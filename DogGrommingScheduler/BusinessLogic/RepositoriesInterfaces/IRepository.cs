using BusinessLogic.Results;

namespace BusinessLogic.RepositoryInterfaces
{
    public interface IRepository<T> where T : class
    {
        Task<Result> AddAsync(T entity);
        Task<Result> RemoveAsync(Guid id);
        Task<Result> RemoveAsync(T entity);
        Task<Result> UpdateAsync(T entity);
        Task<Result<T>> GetByIdAsync(Guid id);
        Task<Result<IEnumerable<T>>> GetAllAsync();
    }
}
