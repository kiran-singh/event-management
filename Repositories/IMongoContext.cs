using MongoDB.Driver;

namespace EventManagement.Repositories;

public interface IMongoContext
{
    IMongoCollection<T> Collection<T>();
}