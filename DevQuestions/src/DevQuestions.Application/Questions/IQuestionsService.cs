using CSharpFunctionalExtensions;
using DevQuestions.Contracts.Questions;
using Shared;

namespace DevQuestions.Application.Questions;

public interface IQuestionsService
{
    Task<Result<Guid, Error[]>> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken);
}