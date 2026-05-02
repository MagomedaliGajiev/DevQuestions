using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Questions.Application.Fails;
using Questions.Domain;
using Shared;
using Shared.Abstractions;
using Shared.Extensions;

namespace Questions.Application.Features.CreateQuestionCommand;

public class CreateQuestionCommandHandler : ICommandHandler<CreateQuestionCommand, Result<Guid, Failure>>
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;

    public CreateQuestionCommandHandler(
        IQuestionsRepository questionsRepository,
        ILogger<QuestionsService> logger)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
    }

    public async Task<Result<Guid, Failure>> Handle(CreateQuestionCommand command, CancellationToken cancellationToken)
    {
        var calculator = new QuestionCalculator();

        var calculateResult = calculator.Calculate();
        if (calculateResult.IsFailure)
        {
            _logger.LogError("Calculation failed for question creation by user {UserId}", command.QuestionDto.UserId);
            return calculateResult.Error;
        }

        // Валидация бизнес логики
        int openedUserQuestionsCount = await _questionsRepository
            .GetOpenedUserQuestionsAsync(command.QuestionDto.UserId, cancellationToken);

        if (openedUserQuestionsCount > 3)
        {
            _logger.LogWarning("User {UserId} has too many open questions ({Count}). Maximum allowed is 3",
                command.QuestionDto.UserId, openedUserQuestionsCount);
            return Errors.Questions.ToManyQuestions().ToFailure();
        }

        // Создание сущности Question
        var questionId = Guid.NewGuid();

        var question = new Question(
            questionId,
            command.QuestionDto.Title,
            command.QuestionDto.Text,
            command.QuestionDto.UserId,
            null,
            command.QuestionDto.TagIds);
        await _questionsRepository.AddAsync(question, cancellationToken);

        _logger.LogInformation("Question created with id {questionId}", questionId);

        return questionId;
    }
}