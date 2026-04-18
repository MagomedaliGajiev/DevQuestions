using CSharpFunctionalExtensions;
using DevQuestions.Application.FullTextSearch;
using DevQuestions.Domain.Questions;
using Shared;

namespace DevQuestions.Infrastructure.ElasticSearch
{
    public class ElasticSearchProvider : ISearchProvider
    {
        public Task<List<Guid>> SearchAsync(string query) => throw new NotImplementedException();

        public async Task<UnitResult<Failure>> IndexQuestionAsync(Question question)
        {
            try
            {
                // _elastic.Search();
            }
            catch (Exception e)
            {
                return Error.Failure("search.error", e.Message).ToFailure();
            }

            return UnitResult.Success<Failure>();
        }
    }
}