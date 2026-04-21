using Questions.Application;
using Questions.Infrastructure.Postgres;
using Questions.Presenters;
using Tags;
using Tags.Contracts;
using Tags.Presenters;

namespace Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services) =>
        services
            .AddWebDependencies()
            .AddApplication()
            .AddPostgresInfrastructure()
            .AddQuestionsModule()
            .AddTagsModule()
            .AddScoped<ITagsContract, TagsModuleContract>();

    private static IServiceCollection AddWebDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}