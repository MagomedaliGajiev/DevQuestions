using Questions.Contracts.Dtos;
using Shared.Abstractions;

namespace Questions.Application.Features.CreateQuestionCommand;

public record CreateQuestionCommandHandler(CreateQuestionDto QuestionDto) : ICommandHandler;