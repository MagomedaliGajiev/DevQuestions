using Microsoft.Extensions.DependencyInjection;
using Questions.Application.FullTextSearch;

namespace Infrastructure.ElasticSearch;

public static class DependencyInjection
{
    public static IServiceCollection AddElasticSearchInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ISearchProvider, ElasticSearchProvider>();

        return services;
    }
}