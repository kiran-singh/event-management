using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SampleCode;

Random random = new();

var users = JsonConvert.DeserializeObject<List<User>>(await File.ReadAllTextAsync("users.json"));
users.ForEach(x => x.Id = Guid.NewGuid());

const string connectionUri = "mongodb://localhost:27017";

MongoClient mongoClient = new MongoClient(connectionUri);

const string databaseName = "Demo";
IMongoDatabase database = mongoClient.GetDatabase(databaseName,
    new MongoDatabaseSettings { GuidRepresentation = GuidRepresentation.Standard, });

IMongoCollection<User> collection = database.GetCollection<User>($"{nameof(User)}s");

try
{
    // CREATE
    if (!collection.Find(x => true).Any())
    {
        // await collection.InsertOneAsync(users[6]);
        await collection.InsertManyAsync(users);
    }

    Console.WriteLine("Users added:");
    
    // READ
    var retrieved = await collection.Find(Builders<User>.Filter.Empty).ToListAsync();
    // var retrieved = await collection.Find(x => true).ToListAsync();

    foreach (var user in retrieved)
    {
        Console.WriteLine(user);
    }
    Console.WriteLine();

    var randomUser = retrieved[random.Next(0, retrieved.Count)];
    var randomId = randomUser.Id;
    Console.WriteLine("Random user:");
    Console.WriteLine(randomUser);
    Console.WriteLine();

    // UPDATE
    UpdateResult updateResult = await collection.UpdateOneAsync(x => x.Id == randomId,
        Builders<User>.Update.Set(x => x.FirstName, "Taylor"));

    Console.WriteLine($"Updated {updateResult.MatchedCount} random user:");
    randomUser = await collection.Find(x => x.Id == randomId).SingleAsync();
    Console.WriteLine(randomUser);
    Console.WriteLine();
    
    UpdateDefinition<User> updates = Builders<User>.Update.Combine(
        Builders<User>.Update.Set(x => x.LastName, "Swift"),
        Builders<User>.Update.Set(x => x.Email, "taySwift99@yahoo.com")
    );
    updateResult = await collection.UpdateOneAsync(x => x.Id == randomId, updates);

    Console.WriteLine($"Fully updated {updateResult.MatchedCount} random user:");
    randomUser = await collection.Find(x => x.Id == randomId).SingleAsync();
    Console.WriteLine(randomUser);

    var idsToDelete = new[] { users[2].Id, users[4].Id, users[6].Id, users[8].Id, users[10].Id };

    // DELETE
    DeleteResult deleteResult = await collection.DeleteOneAsync(x => x.Id == idsToDelete[1]);
    Console.WriteLine($"Deleted {deleteResult?.DeletedCount} users");

    deleteResult = await collection.DeleteManyAsync(x => idsToDelete.Contains(x.Id));
    Console.WriteLine($"Deleted {deleteResult?.DeletedCount} users");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    if (e.InnerException?.Message is { } innerExceptionMessage)
    {
        Console.WriteLine("Inner Exception Message:");
        Console.WriteLine(innerExceptionMessage);
    }
}
finally
{
    DeleteResult deleteResult = collection.DeleteMany(x => true);
    Console.WriteLine($"Deleted {deleteResult.DeletedCount} users");

    await mongoClient.DropDatabaseAsync(databaseName);
    Console.WriteLine($"Database {databaseName} dropped");
}