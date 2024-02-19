using System.Linq.Expressions;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace EventManagement.Domain;

public class Event : EntityBase, IMongoDefinition<Event>
{
    public string? Artist { get; set; }
    public Guid CategoryId { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime Created { get; set; }
    
    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime Date { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int Price { get; set; }
    
    public Expression<Func<Event, bool>> FilterDefinition() => x => x.Id == Id;

    public UpdateDefinition<Event> UpdateDefinition()
    {
        var updateDefinitions = new List<UpdateDefinition<Event>>
        {
            Builders<Event>.Update.Set(x => x.Artist, Artist),
            Builders<Event>.Update.Set(x => x.CategoryId, CategoryId),
            Builders<Event>.Update.Set(x => x.Created, Created),
            Builders<Event>.Update.Set(x => x.Date, Date),
            Builders<Event>.Update.Set(x => x.Description, Description),
            Builders<Event>.Update.Set(x => x.ImageUrl, ImageUrl),
            Builders<Event>.Update.Set(x => x.Name, Name),
            Builders<Event>.Update.Set(x => x.Price, Price),
        };

        return Builders<Event>.Update.Combine(updateDefinitions);
    }
}