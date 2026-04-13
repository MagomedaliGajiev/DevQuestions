namespace DevQuestions.Contracts.Questions.Dtos;

public record GetQuestionDto(string Search, Guid[] TagIds, int Page, int PageSize);