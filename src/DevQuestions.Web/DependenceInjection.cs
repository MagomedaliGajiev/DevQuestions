using DevQuestions.Application;
using DevQuestions.Infrasructure.Postgres;
using DevQuestions.Infrasructure.ElasticSearch;

namespace DevQuestions.Web;

public static class DependenceInjection
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
