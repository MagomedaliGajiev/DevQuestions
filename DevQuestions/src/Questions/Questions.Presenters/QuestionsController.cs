using CSharpFunctionalExtensions;
using Framework.EndpointResults;
using Microsoft.AspNetCore.Mvc;
using Questions.Application.Features.AddAnswerCommand;
using Questions.Application.Features.CreateQuestionCommand;
using Questions.Application.Features.GetQuestionsWithFiltersQuery;
using Questions.Contracts.Dtos;
using Questions.Contracts.Responses;
using Shared;
using Shared.Abstractions;

namespace Questions.Presenters;

[ApiController]
[Route("[controller]")]
public class QuestionsController : ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromServices] ICommandHandler<Guid, CreateQuestionCommand> commandHandler,
        [FromBody] CreateQuestionDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateQuestionCommand(request);

        var result = await commandHandler.Handle(command, cancellationToken);

        return new EndpointResult<Guid>(result);
    }

    [HttpGet]
    public async Task<EndpointResult<QuestionResponse>> Get(
        [FromServices] IQueryHandler<QuestionResponse, GetQuestionsWithFiltersQuery> queryHandler,
        [FromQuery] GetQuestionsDto request,
        CancellationToken cancellationToken)
    {
        var query = new GetQuestionsWithFiltersQuery(request);

        var result = await queryHandler.Handle(query, cancellationToken);

        return new EndpointResult<QuestionResponse>(Result.Success<QuestionResponse, Failure>(result));
    }

    [HttpGet("{questionId:guid}")]
    public Task<EndpointResult<string>> GetById([FromRoute] Guid questionId, CancellationToken cancellationToken)
    {
        return Task.FromResult(new EndpointResult<string>(Result.Success<string, Failure>("Questions get")));
    }

    [HttpPut("{questionId:guid}")]
    public Task<EndpointResult<string>> Update(
        [FromRoute] Guid questionId,
        [FromBody] UpdateQuestionDto request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new EndpointResult<string>(Result.Success<string, Failure>("Question updated")));
    }

    [HttpDelete("{questionId:guid}")]
    public Task<EndpointResult<string>> Delete([FromRoute] Guid questionId, CancellationToken cancellationToken)
    {
        return Task.FromResult(new EndpointResult<string>(Result.Success<string, Failure>("Question deleted")));
    }

    [HttpPut("{questionId:guid}/solution")]
    public Task<EndpointResult<string>> SelectSolution(
        [FromRoute] Guid questionId,
        [FromQuery] Guid answerId,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(new EndpointResult<string>(Result.Success<string, Failure>("Solutions selected")));
    }

    [HttpPost("{questionId:guid}/answers")]
    public async Task<EndpointResult<Guid>> AddAnswer(
        [FromServices] ICommandHandler<Guid, AddAnswerCommand> commandHandler,
        [FromRoute] Guid questionId,
        [FromBody] AddAnswerDto request,
        CancellationToken cancellationToken)
    {
        var command = new AddAnswerCommand(questionId, request);

        var result = await commandHandler.Handle(command, cancellationToken);

        return new EndpointResult<Guid>(result);
    }
}