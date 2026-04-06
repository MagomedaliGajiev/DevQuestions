namespace DevQuestions.Infrastructure.Postgres.Seeders;

public class QuestionSeeder : ISeeder
{
    private readonly QuestionsDbContext _dbContext;

    public QuestionSeeder(QuestionsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SeedAsync() => throw new NotImplementedException();
}