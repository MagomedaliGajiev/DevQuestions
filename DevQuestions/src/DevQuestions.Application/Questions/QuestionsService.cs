using DevQuestions.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DevQuestions.Application.Questions;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly IValidator<CreateQuestionDto> _validator;
    private readonly ILogger<QuestionsService> _logger;

    public QuestionsService(
        IQuestionsRepository questionsRepository,
        IValidator<CreateQuestionDto> validator,
        ILogger<QuestionsService> logger)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Guid> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        // Валидация входных данных
        var validationResult = await _validator.ValidateAsync(questionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Валидация бизнес логики
        int openedUserQuestionsCount = await _questionsRepository
            .GetOpenedUserQuestionsAsync(questionDto.UserId, cancellationToken);

        if (openedUserQuestionsCount > 3)
        {
            throw new Exception("Ползователль не может открыть больше 3 вопросов.");
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