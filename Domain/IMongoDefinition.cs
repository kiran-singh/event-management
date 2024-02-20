using System.Linq.Expressions;
using MongoDB.Driver;

namespace EventManagement.Domain;

public interface IMongoDefinition<T> : IId
{
    Expression<Func<T, bool>> FilterDefinition();

    UpdateDefinition<T> UpdateDefinition();
}