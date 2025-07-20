using Dapper;
using DevQuestions.Application.Database;
using DevQuestions.Application.Questions;
using DevQuestions.Domain.Questions;

namespace DevQuestions.Infrastructure.Postgres.Repositories
{
    public class QuestionsSqlRepository : IQuestionsRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public QuestionsSqlRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Guid> AddAsync(Question question, CancellationToken cancellationToken)
        {
            const string sql = """
                             INSERT INTO questions (id, title, text, user_id, screenshot_id, tags, status)
                             VALUES (@Id, @Title, @Text, @UserId, @Screenshot, @Tags, @Status)
                             """;

            using var connection = _sqlConnectionFactory.Create();

            await connection.ExecuteAsync(sql, new
            {
                Id = question.Id,
                Title = question.Title,
                Text = question.Text,
                UserId = question.UserId,
                ScreensshotId = question.ScreenshotId,
                Tags = question.Tags.ToArray(),
                Status = question.Status,
            });

            return question.Id;
        }

        public Task<Guid> DeleteAsync(Guid questionId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Question?> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetOpenUserQuestionsAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> SaveAsync(Question question, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
