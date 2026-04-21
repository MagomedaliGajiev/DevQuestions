using CSharpFunctionalExtensions;

namespace Shared.Abstractions;

public interface ICommandHandler;

public interface ICommandHandler<TResponse, in TCommand>
    where TCommand : ICommandHandler
{
    Task<Result<TResponse, Failure>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand>
    where TCommand : ICommandHandler
{
    Task<UnitResult<Failure>> Handle(TCommand command, CancellationToken cancellationToken);
}