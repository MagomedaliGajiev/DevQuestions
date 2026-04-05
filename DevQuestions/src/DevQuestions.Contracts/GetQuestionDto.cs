namespace DevQuestions.Contracts;

public record GetQuestionDto(string Search, Guid[] TagIds, int Page, int PageSize);