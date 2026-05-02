using FluentValidation;

namespace Questions.Application.Features.AddAnswerCommand;

public class AddAnswerValidator : AbstractValidator<AddAnswerCommand>
{
    public AddAnswerValidator()
    {
        RuleFor(x => x.AddAnswerDto.Text)
            .NotEmpty().WithMessage("Текст не может быть пустым.")
            .MaximumLength(5000).WithMessage("Текст невалидный.");
    }
}