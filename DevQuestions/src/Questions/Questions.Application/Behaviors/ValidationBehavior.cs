using FluentValidation;
using MediatR;
using Shared;
using Shared.Extensions;

namespace Questions.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var failed = validationResults
            .Where(result => !result.IsValid)
            .ToList();

        if (failed.Count == 0)
        {
            return await next();
        }

        Failure failure = failed.ToErrors();

        // TResponse — это Result<X, Failure>; используем неявное преобразование Failure -> Result<X, Failure>.
        var implicitOp = typeof(TResponse).GetMethod("op_Implicit", [typeof(Failure)]);
        if (implicitOp is null)
        {
            throw new InvalidOperationException(
                $"ValidationBehavior cannot map validation failure to {typeof(TResponse)}.");
        }

        return (TResponse)implicitOp.Invoke(null, [failure])!;
    }
}