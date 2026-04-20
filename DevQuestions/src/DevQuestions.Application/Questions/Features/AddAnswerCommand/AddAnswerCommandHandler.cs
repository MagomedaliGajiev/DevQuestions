using DevQuestions.Application.Abstractions;
using DevQuestions.Contracts.Questions.Dtos;

namespace DevQuestions.Application.Questions.Features.AddAnswerCommand
{
    public record AddAnswerCommandHandler(Guid QuestionId, AddAnswerDto AddAnswerDto) : ICommandHandler;
}