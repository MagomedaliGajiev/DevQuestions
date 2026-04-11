using CSharpFunctionalExtensions;
using DevQuestions.Application.Extensions;
using DevQuestions.Application.FullTextSearch;
using DevQuestions.Application.Questions.Fails;
using DevQuestions.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;

namespace DevQuestions.Application.Questions;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly IValidator<CreateQuestionDto> _validator;
    private readonly ISearchProvider _searchProvider;
    private readonly ILogger<QuestionsService> _logger;

    public QuestionsService(
        IQuestionsRepository questionsRepository,
        IValidator<CreateQuestionDto> validator,
        ISearchProvider searchProvider,
        ILogger<QuestionsService> logger)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
        _searchProvider = searchProvider;
        _validator = validator;
    }

    public async Task<Result<Guid, Failure>> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        // Валидация входных данных
        var validationResult = await _validator.ValidateAsync(questionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation failed for question creation by user {UserId}. Errors: {Errors}",
                questionDto.UserId, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            return validationResult.ToErrors();
        }

        var calculator = new QuestionCalculator();

        var calculateResult = calculator.Calculate();
        if (calculateResult.IsFailure)
        {
            _logger.LogError("Calculation failed for question creation by user {UserId}", questionDto.UserId);
            return calculateResult.Error;
        }

        // Валидация бизнес логики
        int openedUserQuestionsCount = await _questionsRepository
            .GetOpenedUserQuestionsAsync(questionDto.UserId, cancellationToken);

        if (openedUserQuestionsCount > 3)
        {
            _logger.LogWarning("User {UserId} has too many open questions ({Count}). Maximum allowed is 3",
                questionDto.UserId, openedUserQuestionsCount);
            return Errors.Questions.ToManyQuestions().ToFailure();
        }

        // Создание сущности Question
        var questionId = Guid.NewGuid();

        var question = new Question(
            questionId,
            questionDto.Title,
            questionDto.Text,
            questionDto.UserId,
            null,
            questionDto.TagIds);
        await _questionsRepository.AddAsync(question, cancellationToken);

        _logger.LogInformation("Question created with id {questionId}", questionId);

        return questionId;
    }

    // public async Task<IActionResult> Delete(Guid questionId, CancellationToken cancellationToken)
    // {
    //
    // }
    //
    // public async Task<IActionResult> SelectSolution(
    //     Guid questionId,
    //     Guid answerId,
    //     CancellationToken cancellationToken)
    // {
    //
    // }
    //
    // public async Task<IActionResult> AddAnswer(
    //     Guid questionId,
    //     AddAnswerDto request,
    //     CancellationToken cancellationToken)
    // {
    //
    // }
}

public class QuestionCalculator
    {
        public Result<int, Failure> Calculate()
        {
            // operation
            return Error.Failure("", "").ToFailure();
        }
    }