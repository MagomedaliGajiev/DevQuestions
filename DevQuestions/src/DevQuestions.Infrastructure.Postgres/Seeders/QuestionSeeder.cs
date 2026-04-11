using DevQuestions.Infrastructure.Postgres.Questions;

namespace DevQuestions.Infrastructure.Postgres.Seeders;

public class QuestionSeeder : ISeeder
{
    private readonly QuestionsReadDbContext _readDbContext;

    public QuestionSeeder(QuestionsReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public Task SeedAsync() => throw new NotImplementedException();
}