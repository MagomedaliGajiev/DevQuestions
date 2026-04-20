using DevQuestions.Application;
using DevQuestions.Infrastructure.Communication;
using DevQuestions.Infrastructure.ElasticSearch;
using DevQuestions.Infrastructure.Postgres;
using DevQuestions.Infrastructure.S3;

namespace DevQuestions.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProgramDependencies(this IServiceCollection services) =>
            services
                .AddWebDependencies()
                .AddApplication()
                .AddPostgresInfrastructure()
                .AddElasticSearchInfrastructure()
                .AddCommunicationInfrastructure()
                .AddS3Infrastructure();

        private static IServiceCollection AddWebDependencies(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();

            return services;
        }
    }
}