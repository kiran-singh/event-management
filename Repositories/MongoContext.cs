using EventManagement.Domain;
using MongoDB.Driver;

namespace EventManagement.Repositories;

public class MongoContext : IMongoContext
{
    private static readonly IDictionary<Type, string> Mappings = new Dictionary<Type, string>
    {
        {typeof(Category), "Categories"},
    };

    private readonly IMongoDatabase _mongoDatabase;

    public MongoContext(IMongoDatabase mongoDatabase) => _mongoDatabase = mongoDatabase;

    public IMongoCollection<T> Collection<T>() =>
        _mongoDatabase.GetCollection<T>(Mappings.TryGetValue(typeof(T), out var value) ? value : $"{typeof(T).Name}s");
}