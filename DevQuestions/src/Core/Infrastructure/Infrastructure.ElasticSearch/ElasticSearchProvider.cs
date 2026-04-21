using CSharpFunctionalExtensions;
using Questions.Application.FullTextSearch;
using Questions.Domain;
using Shared;

namespace Infrastructure.ElasticSearch;

public class ElasticSearchProvider : ISearchProvider
{
    public Task<List<Guid>> SearchAsync(string query) => throw new NotImplementedException();

    public Task<UnitResult<Failure>> IndexQuestionAsync(Question question) => throw new NotImplementedException();
}