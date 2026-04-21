using Microsoft.Extensions.DependencyInjection;
using Shared.FilesStorage;

namespace Infrastructure.S3;

public static class DependencyInjection
{
    public static IServiceCollection AddS3Infrastructure(this IServiceCollection services)
    {
        services.AddScoped<IFilesProvider, SProvider>();
        return services;
    }
}