using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Questions.Application.Fails;
using Questions.Contracts.Dtos;
using Questions.Domain;
using Shared;
using Shared.Abstractions;
using Shared.Database;
using Shared.Extensions;

namespace Questions.Application.Features.AddAnswerCommand;

public class AddAnswerCommandHandler : ICommandHandler<Guid, AddAnswerCommand>
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ITransactionManager _transactionManager;
    // private readonly IUsersCommunicationService _usersCommunicationService;
    private readonly IValidator<AddAnswerDto> _validator;
    private readonly ILogger<QuestionsService> _logger;

    public AddAnswerCommandHandler(
        IQuestionsRepository questionsRepository,
        ITransactionManager transactionManager,
        // IUsersCommunicationService usersCommunicationService,
        ILogger<QuestionsService> logger,
        IValidator<AddAnswerDto> validator)
    {
        _questionsRepository = questionsRepository;
        _transactionManager = transactionManager;
        // _usersCommunicationService = usersCommunicationService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, Failure>> Handle(
        AddAnswerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(command.AddAnswerDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }

        // var usersRatingResult = await _usersCommunicationService.GetUserRatingAsync(commandHandler.AddAnswerDto.UserId, cancellationToken);
        // if (usersRatingResult.IsFailure)
        // {
        //     return usersRatingResult.Error;
        // }

        // if (usersRatingResult.Value <= 0)
        // {
        //     _logger.LogError("User with id {userId} has no rating", commandHandler.AddAnswerDto.UserId);
        //     return Errors.Questions.NotEnoughRating();
        // }

        var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken);

        (_, bool isFailure, Question? question, Failure? error) = await _questionsRepository.GetByIdAsync(command.QuestionId, cancellationToken);
        if (isFailure)
        {
            return error;
        }

        var answer = new Answer(Guid.NewGuid(), command.AddAnswerDto.UserId, command.AddAnswerDto.Text, command.QuestionId);

        question.Answers.Add(answer);

        await _questionsRepository.SaveAsync(question, cancellationToken);

        transaction.Commit();

        _logger.LogInformation("Answer added with id {answerId} to question {questionId}", answer.Id, command.QuestionId);

        return answer.Id;
    }
}