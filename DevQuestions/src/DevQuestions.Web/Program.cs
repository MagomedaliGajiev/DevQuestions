using DevQuestions.Infrastructure.Postgres;
using DevQuestions.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramDependencies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DevQuestions"));
}

app.MapControllers();

using var scope = app.Services.CreateAsyncScope();

var seeders = scope.ServiceProvider.GetServices<ISeeder>();

foreach (var seeder in seeders)
{
    await seeder.SeedAsync();
}

app.Run();