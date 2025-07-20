using DevQuestions.Application.FullTextSearch;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Infrasructure.ElasticSearch;

public static class DependenceInjection
{
    public static IServiceCollection AddElasticSearchInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISearchProvider, ElasticSearchProvider>();

        return services;
    }
}
