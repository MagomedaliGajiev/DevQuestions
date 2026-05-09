using Shared;

namespace Web.EndpointResults;

public sealed class FailureResult : IResult
{
    private readonly Failure _failure;

    public FailureResult(Failure failure)
    {
        _failure = failure;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        if (!_failure.Any())
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            return httpContext.Response.WriteAsJsonAsync(Envelope.Failure(_failure));
        }

        var distinctFailureTypes = _failure
            .Select(x => x.Type)
            .Distinct()
            .ToList();


        int statusCode = distinctFailureTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCodeForErrorType(distinctFailureTypes.First());

        var envelope = Envelope.Failure(_failure);
        httpContext.Response.StatusCode = statusCode;

        return httpContext.Response.WriteAsJsonAsync(envelope);
    }

    private static int GetStatusCodeForErrorType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.VALIDATION => StatusCodes.Status400BadRequest,
            ErrorType.NOT_FOUND => StatusCodes.Status404NotFound,
            ErrorType.CONFLICT => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
}