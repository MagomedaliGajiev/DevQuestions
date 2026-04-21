using DevQuestions.Application.Communication;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Infrastructure.Communication;

public static class DependencyInjection
{
    public static IServiceCollection AddCommunicationInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUsersCommunicationService, UsersCommunicationService>();
        return services;
    }
}