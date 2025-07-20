using DevQuestions.Application.Questions;
using DevQuestions.Infrasructure.Postgres.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Infrasructure.Postgres;

public static class DependenceInjection
{
    public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<QuestionsDbContext>();

        services.AddScoped<IQuestionsRepository, QuestionsEfRepository>();

        return services;
    }
}
