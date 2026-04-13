using DevQuestions.Contracts.Questions;
using DevQuestions.Contracts.Questions.Dtos;
using FluentValidation;

namespace DevQuestions.Application.Questions.Features.AddAnswer;

public class AddAnswerValidator : AbstractValidator<AddAnswerDto>
{
    public AddAnswerValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Текст не может быть пустым.")
            .MaximumLength(5000).WithMessage("Текст невалидный.");
    }
}