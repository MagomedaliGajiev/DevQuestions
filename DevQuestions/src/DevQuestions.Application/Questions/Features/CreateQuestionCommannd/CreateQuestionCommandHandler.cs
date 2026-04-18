using DevQuestions.Application.Abstractions;
using DevQuestions.Contracts.Questions.Dtos;

namespace DevQuestions.Application.Questions.Features.CreateQuestionCommannd
{
    public record CreateQuestionCommandHandler(CreateQuestionDto QuestionDto) : ICommandHandler;
}