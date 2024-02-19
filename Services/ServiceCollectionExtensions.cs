using Microsoft.Extensions.DependencyInjection;

namespace EventManagement.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection collection)
        => collection.AddScoped(typeof(IMongoService<>), typeof(MongoService<>));
}