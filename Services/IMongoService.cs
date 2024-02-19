using System.Linq.Expressions;
using EventManagement.Domain;

namespace EventManagement.Services;

public interface IMongoService<T> where T : IMongoDefinition<T>
{
    Task<bool> AddAsync<K>(K model);

    Task<K> Get<K>(Expression<Func<T, bool>> predicate);
        
    Task<IList<K>> GetListAsync<K>(MongoQueryModel model);
}