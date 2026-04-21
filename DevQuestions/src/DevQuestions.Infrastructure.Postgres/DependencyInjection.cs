using DevQuestions.Application.Tags;
using DevQuestions.Infrastructure.Postgres.Database;
using DevQuestions.Infrastructure.Postgres.Tags;
using Microsoft.Extensions.DependencyInjection;
using Questions.Application;
using Questions.Infrastructure.Postgres;
using Shared.Database;

namespace DevQuestions.Infrastructure.Postgres;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            services.AddScoped<IQuestionsRepository, QuestionsSqlRepository>();
            services.AddScoped<ITransactionManager, TransactionManager>();
            services.AddScoped<ITagsReadDbContext, TagsReadDbContext>();
            services.AddScoped<IQuestionsReadDbContext, QuestionsReadDbContext>();

            return services;
        }
    }
