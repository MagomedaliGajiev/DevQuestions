
using DevQuestions.Application.FullTextSearch;
using DevQuestions.Domain.Questions;

namespace DevQuestions.Infrasructure.ElasticSearch;

public class ElasticSearchProvider : ISearchProvider
{
    public Task IndexQuestionAssync(Question question) => throw new NotImplementedException();

    public Task<List<Guid>> SearchAsync(string query) => throw new NotImplementedException();
}
