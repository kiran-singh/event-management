using MongoDB.Driver;

namespace EventManagement.Domain;

public class MongoQueryModel
{
    public SortDirection Direction { get; set; } = SortDirection.Ascending;
        
    public int PerPage { get; set; } = 10;
        
    public string SortProperty { get; set; }
        
    public IFindFluent<T, T> Process<T>(IFindFluent<T, T> query)
    {
        if (!string.IsNullOrWhiteSpace(SortProperty))
            query = Direction == SortDirection.Ascending
                ? query.Sort(Builders<T>.Sort.Ascending(SortProperty))
                : query.Sort(Builders<T>.Sort.Descending(SortProperty));

        return PerPage > 0 ? query.Limit(PerPage) : query;
    }
}