using EventManagement.Domain;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventManagement.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class, IId, IMongoDefinition<T>
{
    protected readonly IMongoCollection<T> MongoCollection;
    private readonly ILogger<BaseRepository<T>> _logger;

    public BaseRepository(IMongoContext mongoContext, ILogger<BaseRepository<T>> logger)
    {
        MongoCollection = mongoContext.Collection<T>();
        _logger = logger;
    }

    public async Task<bool> AddOrUpdateAsync(T entity)
    {
        var updateResult = await MongoCollection.UpdateOneAsync(entity.FilterDefinition(), entity.UpdateDefinition(),
            new UpdateOptions {IsUpsert = true});

        var valid = updateResult.IsAcknowledged &&
                    (updateResult.ModifiedCount > 0 || updateResult.UpsertedId != BsonNull.Value);

        if (!valid)
            _logger.LogWarning("Failure adding {Name}: {Entity}", typeof(T).Name, entity);
        
        return valid;
    }

    public async Task DeleteAsync(Guid id) => await MongoCollection.DeleteOneAsync(x => x.Id == id);
    
    public async Task<T?> GetByIdAsync(Guid id)
        => await MongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<IReadOnlyList<T>> ListAllAsync()
        => await MongoCollection.Find(FilterDefinition<T>.Empty).ToListAsync();
}