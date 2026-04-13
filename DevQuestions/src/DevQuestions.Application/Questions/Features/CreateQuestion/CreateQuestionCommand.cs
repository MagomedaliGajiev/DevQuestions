using DevQuestions.Application.Abstractions;
using DevQuestions.Contracts.Questions;
using DevQuestions.Contracts.Questions.Dtos;

namespace DevQuestions.Application.Questions.Features.CreateQuestion;

public record CreateQuestionCommand(CreateQuestionDto QuestionDto) : ICommand;