using Shared;

namespace DevQuestions.Application.Questions.Fails;

public partial class Errors
{
    public static class Questions
    {
        public static Error TooManyQuestions() =>
            Error.Failure("qustions.too.many", "Пользователь не может создать больше 3 вопросов");
    }
}
