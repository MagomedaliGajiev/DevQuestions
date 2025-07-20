using DevQuestions.Application.Questions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Application;

public static class DependenceInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependenceInjection).Assembly);

        services.AddScoped<IQuestionsService, QuestionsService>();

        return services;
    }
}
