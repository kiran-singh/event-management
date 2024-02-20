using EventManagement.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace EventManagement.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection collection, IConfiguration configuration)
    {
        BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);
        
        var connectionString = configuration.GetConnectionString(nameof(EventManagement));

        collection.AddSingleton<MongoClient>(_ => new MongoClient(connectionString));

        collection.AddScoped<IMongoContext, MongoContext>();

        collection.AddScoped<IMongoDatabase>(sp =>
        {
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoDatabase = sp.GetRequiredService<MongoClient>()
                .GetDatabase(mongoUrl.DatabaseName,
                    new MongoDatabaseSettings { GuidRepresentation = GuidRepresentation.Standard, });

            mongoDatabase.GetCollection<Event>($"{nameof(Event)}s")
                .Indexes.CreateOne(
                    new CreateIndexModel<Event>(Builders<Event>.IndexKeys.Ascending(x => x.Name),
                        new CreateIndexOptions { Background = true }));

            DataHelper.AddSeedData(new MongoContext(mongoDatabase));

            return mongoDatabase;
        });

        collection.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        
        return collection;
    }
}