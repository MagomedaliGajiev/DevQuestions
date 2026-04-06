namespace DevQuestions.Infrastructure.Postgres;

public interface ISeeder
{
    Task SeedAsync();
}