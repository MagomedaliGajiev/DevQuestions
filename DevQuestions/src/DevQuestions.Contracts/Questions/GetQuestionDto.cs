namespace DevQuestions.Contracts.Questions;

public record GetQuestionDto(string Search, Guid[] TagIds, int Page, int PageSize);