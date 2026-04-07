using DevQuestions.Application;
using DevQuestions.Infrastructure.ElasticSearch;
using DevQuestions.Infrastructure.Postgres;

namespace DevQuestions.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services) =>
        services
            .AddWebDependencies()
            .AddApplication()
            .AddPostgresInfrastructure()
            .AddElasticSearchInfrastructure();

    private static IServiceCollection AddWebDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}