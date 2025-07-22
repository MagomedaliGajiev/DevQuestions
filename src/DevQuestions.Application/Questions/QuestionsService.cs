using DevQuestions.Application.Extensions;
using DevQuestions.Application.FullTextSearch;
using DevQuestions.Application.Questions.Fails.Exceptions;
using DevQuestions.Contracts.Questions;
using DevQuestions.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DevQuestions.Application.Questions;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ISearchProvider _searchProvider;
    private readonly ILogger<QuestionsService> _logger;
    private readonly IValidator<CreateQuestionDto> _validator;

    public QuestionsService(
        IQuestionsRepository questionsRepository,
        IValidator<CreateQuestionDto> validator,
        ISearchProvider searchProvider,
        ILogger<QuestionsService> logger)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
        _validator = validator;
        _searchProvider = searchProvider;
    }

    public async Task<Guid> Create(
        CreateQuestionDto questionDto,
        CancellationToken cancellationToken)
    {
        // проверка валидности
        var validationResult = await _validator.ValidateAsync(questionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
                throw new QuestionValidationException(validationResult.ToErrors());
        }

        var calculator = new QuestionCalculataor();

        calculator.Calculate();

        // валидация бизнес логики
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenUserQuestionsAsync(questionDto.UserId, cancellationToken);

        if (openUserQuestionsCount > 3)
        {
            throw new TooManyQuestionsException();
        }

        var existedQuestion = await _questionsRepository.GetByIdAsync(Guid.Empty, cancellationToken);

        // создание сущности Question
        var questionId = Guid.NewGuid();

        var question = new Question(
             questionId,
             questionDto.Title,
             questionDto.Text,
             questionDto.UserId,
             null,
             questionDto.TagIds);

        // сохранение сущности Question в базе данных
        await _questionsRepository.AddAsync(question, cancellationToken);

        await _searchProvider.IndexQuestionAssync(question);

        // Логирование об успешном не успешном сохранение данных
        _logger.LogInformation("Question created with id {questionId}", questionId);

        return questionId;
    }

    //public async Task<IActionResult> Update(
    //    Guid questionId,
    //    UpdateQuestionDto request,
    //    CancellationToken cancellationToken)
    //{

    //}

    //public async Task<IActionResult> Delete(
    //    Guid questionId,
    //    CancellationToken cancellationToken)
    //{

    //}

    //public async Task<IActionResult> SelectSolution(
    //    Guid questionId,
    //    Guid answerId,
    //    CancellationToken cancellationToken)
    //{

    //}

    //public async Task<IActionResult> AddAnswer(
    //    Guid questionId,
    //    AddAnswerDto request,
    //    CancellationToken cancellationToken)
    //{

    //}
}

public class QuestionCalculataor
{
    public void Calculate()
    {
        throw new NotImplementedException();
    }
}
