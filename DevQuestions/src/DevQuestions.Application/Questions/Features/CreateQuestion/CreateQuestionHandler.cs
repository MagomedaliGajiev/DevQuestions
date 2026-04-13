using CSharpFunctionalExtensions;
using DevQuestions.Application.Abstractions;
using DevQuestions.Application.Extensions;
using DevQuestions.Application.Questions.Fails;
using DevQuestions.Contracts.Questions;
using DevQuestions.Contracts.Questions.Dtos;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;

namespace DevQuestions.Application.Questions.Features.CreateQuestion;

public class CreateQuestionHandler : ICommandHandler<Guid, CreateQuestionCommand>
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;
    private readonly IValidator<CreateQuestionDto> _validator;

    public CreateQuestionHandler(
        IQuestionsRepository questionsRepository,
        ILogger<QuestionsService> logger,
        IValidator<CreateQuestionDto> validator)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Guid, Failure>> Handle(CreateQuestionCommand command, CancellationToken cancellationToken)
    {
        // Валидация входных данных
        var validationResult = await _validator.ValidateAsync(command.QuestionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for question creation by user {UserId}. Errors: {Errors}",
                command.QuestionDto.UserId, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            return validationResult.ToErrors();
        }

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