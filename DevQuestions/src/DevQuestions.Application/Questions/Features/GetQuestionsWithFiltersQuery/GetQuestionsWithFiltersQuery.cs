using DevQuestions.Application.Abstractions;
using DevQuestions.Application.Questions.Dtos;

namespace DevQuestions.Application.Questions.Features.GetQuestionsWithFiltersQuery;

public record GetQuestionsWithFiltersQuery(GetQuestionsDto Dto) : IQuery;