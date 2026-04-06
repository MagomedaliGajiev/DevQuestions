using DevQuestions.Domain.Questions;
using Microsoft.EntityFrameworkCore;

namespace DevQuestions.Infrastructure.Postgres;

public class QuestionsDbContext : DbContext
{
    public DbSet<Question> Questions { get; set; }
}