using System.Linq.Expressions;
using MongoDB.Driver;

namespace EventManagement.Domain;

public class Category : EntityBase, IMongoDefinition<Category>
{
    public Expression<Func<Category, bool>> FilterDefinition() => x => x.Id == Id;

    public UpdateDefinition<Category> UpdateDefinition()
        => Builders<Category>.Update.Combine(Builders<Category>.Update.Set(c => c.Name, Name));
}