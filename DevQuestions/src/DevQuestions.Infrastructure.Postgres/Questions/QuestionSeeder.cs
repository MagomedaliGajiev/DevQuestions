namespace DevQuestions.Infrastructure.Postgres.Questions
{
    public class QuestionSeeder : ISeeder
    {
        private readonly QuestionsReadDbContext _readDbContext;

        public QuestionSeeder(QuestionsReadDbContext readDbContext)
        {
            _readDbContext = readDbContext;
        }

        public Task SeedAsync() => throw new NotImplementedException();
    }
}