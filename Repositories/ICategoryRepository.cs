using EventManagement.Domain;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace EventManagement.Repositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<IList<(Category, IEnumerable<Event>)>> ListAllWithEventsAsync(bool includePastEvents);
}

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    private readonly IMongoContext _mongoContext;

    public CategoryRepository(IMongoContext mongoContext, ILogger<CategoryRepository> logger) : base(mongoContext, logger)
    {
        _mongoContext = mongoContext;
    }

    public async Task<IList<(Category, IEnumerable<Event>)>> ListAllWithEventsAsync(bool includePastEvents)
    {
        var query = await (from category in MongoCollection.AsQueryable()
            join evt in _mongoContext.Collection<Event>().AsQueryable()
                on category.Id equals evt.CategoryId
                into events
            select new { category, events }).ToListAsync();
    
        return query.Select(x =>
            (x.category, includePastEvents ? x.events : x.events.Where(e => e.Date >= DateTime.Today)))
            .ToList();
    }
}