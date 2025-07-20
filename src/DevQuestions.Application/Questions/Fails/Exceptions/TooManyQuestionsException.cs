using DevQuestions.Application.Exceptions;

namespace DevQuestions.Application.Questions.Fails.Exceptions;

public class TooManyQuestionsException : BadRequestException
{
    public TooManyQuestionsException()
        : base([Errors.Questions.TooManyQuestions()])
    {
    }
}

