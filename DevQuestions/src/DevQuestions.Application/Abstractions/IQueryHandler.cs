namespace DevQuestions.Application.Abstractions;

public interface IQueryHandler;

public interface IQueryHandler<TResponse, in TQuery>
    where TQuery : IQueryHandler
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}