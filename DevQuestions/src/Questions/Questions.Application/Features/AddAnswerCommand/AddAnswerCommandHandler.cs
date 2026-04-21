using Questions.Contracts.Dtos;
using Shared.Abstractions;

namespace Questions.Application.Features.AddAnswerCommand;

public record AddAnswerCommandHandler(Guid QuestionId, AddAnswerDto AddAnswerDto) : ICommandHandler;