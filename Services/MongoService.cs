using System.Linq.Expressions;
using AutoMapper;
using EventManagement.Domain;
using EventManagement.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventManagement.Services;

public class MongoService<T> : IMongoService<T> where T : IMongoDefinition<T>
{
    protected readonly IMongoCollection<T> Collection;
    protected readonly IMapper Mapper;
    private readonly ILogger<MongoService<T>> _logger;

    public MongoService(IMapper mapper, IMongoContext mongoContext, ILogger<MongoService<T>> logger)
    {
        Mapper = mapper;
        _logger = logger;
        Collection = mongoContext.Collection<T>();
    }

    public async Task<bool> AddAsync<K>(K model)
    {
        var entity = Mapper.Map<K, T>(model);

        return await SaveAsync(model, entity);
    }

    public virtual async Task<K> Get<K>(Expression<Func<T, bool>> predicate)
    {
        if (await Collection.Find(predicate).SingleOrDefaultAsync() is { } item)
            return Mapper.Map<T, K>(item);

        return default;
    }

    public async Task<IList<K>> GetListAsync<K>(MongoQueryModel model)
    {
        var query = Collection.Find(FilterDefinition<T>.Empty);
            
        var entities = await model.Process(query).ToListAsync();

        var result = Mapper.Map<IList<T>, IList<K>>(entities);

        return result;
    }
        
    public virtual async Task<bool> SaveAsync<K>(K model, T entity)
    {
        var updateResult = await Collection.UpdateOneAsync(entity.FilterDefinition(), entity.UpdateDefinition(),
            new UpdateOptions {IsUpsert = true});

        var valid = updateResult.IsAcknowledged &&
                    (updateResult.ModifiedCount > 0 || updateResult.UpsertedId != BsonNull.Value);

        if (!valid)
            _logger.LogWarning($"Failure adding {typeof(T).Name}: {model}");

        return valid;
    }
}