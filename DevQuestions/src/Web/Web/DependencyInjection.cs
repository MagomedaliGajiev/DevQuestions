using Questions.Application;
using Questions.Infrastructure.Postgres;

namespace Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services) =>
        services
            .AddWebDependencies()
            .AddApplication()
            .AddPostgresInfrastructure();
    //    .AddElasticSearchInfrastructure()
    //    .AddCommunicationInfrastructure()
    //     .AddS3Infrastructure();

    private static IServiceCollection AddWebDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}