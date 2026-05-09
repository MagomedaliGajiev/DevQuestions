using System.Reflection;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.Metadata;
using Shared;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Web.EndpointResults;

public sealed class EndpointResult<TValue> : IResult, IEndpointMetadataProvider
{
    private readonly IResult _result;

    public EndpointResult(Result<TValue, Failure> result)
    {
        _result = result.IsSuccess
            ? new SuccessResult<TValue>(result.Value)
            : new FailureResult(result.Error);
    }

    public Task ExecuteAsync(HttpContext httpContext) =>
        _result.ExecuteAsync(httpContext);

    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder) => throw new NotImplementedException();
}