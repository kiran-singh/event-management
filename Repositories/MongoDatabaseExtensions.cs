using MongoDB.Driver;

namespace EventManagement.Repositories;

public static class MongoDatabaseExtensions
{
    public static IMongoCollection<T>? ReturnCollectionIfEmpty<T>(this IMongoContext mongoContext)
    {
        var collection = mongoContext.Collection<T>();

        return collection.Find(FilterDefinition<T>.Empty).FirstOrDefault() == null ? collection : null;
    }
}