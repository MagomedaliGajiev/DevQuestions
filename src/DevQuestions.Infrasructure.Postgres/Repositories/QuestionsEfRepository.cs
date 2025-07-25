﻿using DevQuestions.Application.Questions;
using DevQuestions.Domain.Questions;
using Microsoft.EntityFrameworkCore;

namespace DevQuestions.Infrasructure.Postgres.Repositories;

public class QuestionsEfRepository : IQuestionsRepository
{
    private readonly QuestionsDbContext _dbContext;

    public QuestionsEfRepository(QuestionsDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Guid> AddAsync(Question question, CancellationToken cancellationToken)
    {
        await _dbContext.Questions.AddAsync(question, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return question.Id;
    }

    public Task<Guid> DeleteAsync(Guid questionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Question?> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        var question = await _dbContext.Questions
            .Include(q => q.Answers)
            .Include(q => q.Solution)
           .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);

        return question;
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
