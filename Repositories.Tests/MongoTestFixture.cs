using EventManagement.Domain;
using Mongo2Go;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Repositories.Tests;

public class MongoTestFixture : IDisposable
{
    private readonly MongoDbRunner _runner;

    public MongoTestFixture()
    {
        BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);

        _runner = MongoDbRunner.Start(singleNodeReplSet: false);
        var client = new MongoClient(_runner.ConnectionString);
        Database = client.GetDatabase(nameof(EventManagement));
            
        Events = Database.GetCollection<Event>(nameof(Events));
    }
        
    public IMongoCollection<Event> Events { get; }
    
    public IMongoDatabase Database { get; }
    
    public void Dispose() => _runner?.Dispose();
}