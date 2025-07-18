﻿using DevQuestions.Application;

namespace DevQuestions.Web
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddProgramDependencies(this IServiceCollection services) =>
            services
                .AddWebDependencies()
                .AddApplication();
 

        private static IServiceCollection AddWebDependencies(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddOpenApi();

            return services;
        }
    }
}
