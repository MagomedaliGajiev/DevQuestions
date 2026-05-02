using FluentValidation;

namespace Questions.Application.Features.CreateQuestionCommand;

public class CreateQuestionValidator : AbstractValidator<CreateQuestionCommand>
{
    public CreateQuestionValidator()
    {
        RuleFor(x => x.QuestionDto.Title)
            .NotEmpty().WithMessage("Заголовок не долженн быть пустым.")
            .MaximumLength(500)
            .WithMessage("Заголовок невалидный.");

        RuleFor(x => x.QuestionDto.Text)
            .NotEmpty().WithMessage("Текст не может быть пустым.")
            .MaximumLength(5000).WithMessage("Текст невалидный.");

        RuleFor(x => x.QuestionDto.UserId).NotEmpty();
    }
}