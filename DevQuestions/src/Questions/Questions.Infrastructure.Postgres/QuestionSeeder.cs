using Shared.Database;

namespace Questions.Infrastructure.Postgres;

public class QuestionSeeder : ISeeder
{
    private readonly QuestionsDbContext _dbContext;

    public QuestionSeeder(QuestionsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SeedAsync() => throw new NotImplementedException();
}