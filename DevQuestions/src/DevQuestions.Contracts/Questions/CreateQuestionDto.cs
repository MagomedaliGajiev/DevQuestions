namespace DevQuestions.Contracts.Questions;

public record CreateQuestionDto(string Title, string Body, Guid UserId, Guid[] TagsIds);