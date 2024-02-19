using EventManagement.Domain;

namespace EventManagement.Repositories;

public interface IBaseRepository<T> where T : class, IId
{
    Task<bool> AddOrUpdateAsync(T entity);
    
    Task DeleteAsync(Guid id);
    
    Task<T?> GetByIdAsync(Guid id);
    
    Task<IReadOnlyList<T>> ListAllAsync();
}