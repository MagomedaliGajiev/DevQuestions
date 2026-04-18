using DevQuestions.Application.FilesStorage;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Infrastructure.S3;

public static class DependencyInjection
{
    public static IServiceCollection AddS3Infrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFilesProvider, SProvider>();
        return services;
    }
}