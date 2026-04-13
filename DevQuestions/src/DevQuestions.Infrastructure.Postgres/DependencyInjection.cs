using DevQuestions.Application.Database;
using DevQuestions.Application.Questions;
using DevQuestions.Infrastructure.Postgres.Database;
using DevQuestions.Infrastructure.Postgres.Questions;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IQuestionsRepository, QuestionsSqlRepository>();
        services.AddScoped<ITransactionManager, TransactionManager>();

        return services;
    }
}