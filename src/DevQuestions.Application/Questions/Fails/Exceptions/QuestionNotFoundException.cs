using DevQuestions.Application.Exceptions;
using Shared;

namespace DevQuestions.Application.Questions.Fails.Exceptions;

public class QuestionNotFoundException : NotFoundException
{
    public QuestionNotFoundException(Error[] error)
        : base(error)
    {
    }
}
