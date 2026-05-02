using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Questions.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<IQuestionsService, QuestionsService>();

        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblies(assembly);
        });

    //     services.Scan(scan => scan.FromAssemblies(assembly)
    //         .AddClasses(classes => classes
    //             .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
    //         .AsSelfWithInterfaces()
    //         .WithScopedLifetime());
    //
        // services.Scan(scan => scan.FromAssemblies(assembly)
        // .AddClasses(classes => classes
        //     .AssignableToAny(typeof(IQueryHandler<,>)))
        // .AsSelfWithInterfaces()
        // .WithScopedLifetime());

        return services;
    }
}