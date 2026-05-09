using System.Reflection;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Shared;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Framework.EndpointResults;

public sealed class EndpointResult<TValue> : IResult, IEndpointMetadataProvider
{
    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(builder);

        string[] contentTypes = ["application/json"];

        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status200OK, typeof(Envelope<TValue>), contentTypes));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status400BadRequest, typeof(Envelope), contentTypes));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status404NotFound, typeof(Envelope), contentTypes));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status409Conflict, typeof(Envelope), contentTypes));
        builder.Metadata.Add(new ProducesResponseTypeMetadata(StatusCodes.Status500InternalServerError, typeof(Envelope), contentTypes));
    }

    private readonly IResult _result;

    public EndpointResult(Result<TValue, Failure> result)
    {
        _result = result.IsSuccess
            ? new SuccessResult<TValue>(result.Value)
            : new FailureResult(result.Error);
    }

    public Task ExecuteAsync(HttpContext httpContext) =>
        _result.ExecuteAsync(httpContext);
}