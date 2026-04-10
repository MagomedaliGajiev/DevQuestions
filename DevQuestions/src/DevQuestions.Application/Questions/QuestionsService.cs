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
            validationResult.ToErrors();
        }

        var calculator = new QuestionCalculator();

        var calculateResult = calculator.Calculate();
        if (calculateResult.IsFailure)
        {
            return calculateResult.Error;
        }

        // Валидация бизнес логики
        int openedUserQuestionsCount = await _questionsRepository
            .GetOpenedUserQuestionsAsync(questionDto.UserId, cancellationToken);

        if (openedUserQuestionsCount > 3)
        {
            return Errors.Questions.ToManyQuestions().ToFailure();
        }

        // Создание сущнноости Question
        var questionId = Guid.NewGuid();

        var question = new Question(
            questionId,
            questionDto.Title,
            questionDto.Text,
            questionDto.UserId,
            null,
            questionDto.TagIds);

        // Сохранение сущности Question в базе данных
        await _questionsRepository.AddAsync(question, cancellationToken);
        

        var indexResult = await _searchProvider.IndexQuestionAsync(question);
        if (indexResult.IsFailure)
        {
            return indexResult.Error;
        }

        // Логировние об успешном неуспешном схранении
        _logger.LogInformation("Question created with id {questionId}", questionId);

        return questionId;
    }

    // public async Task<IActionResult> Update(
    //     Guid questionId,
    //     UpdateQuestionDto request,
    //     CancellationToken cancellationToken)
    // {
    //
    // }
    //
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
    public UnitResult<Failure> Calculate()
    {
        // operation
        return Error.Failure("", "").ToFailure();
    }
}