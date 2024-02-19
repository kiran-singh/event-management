using System.Linq.Expressions;
using MongoDB.Driver;

namespace EventManagement.Domain;

public interface IMongoDefinition<T>
{
    Expression<Func<T, bool>> FilterDefinition();

    UpdateDefinition<T> UpdateDefinition();
}