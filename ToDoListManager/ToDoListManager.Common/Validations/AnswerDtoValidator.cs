using FluentValidation;
using ToDoListManager.Common.Dtos;

namespace ToDoListManager.Common.Validations;

public class AnswerDtoValidator : AbstractValidator<AnswerDto>
{
    public AnswerDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(5);

        RuleFor(x => x.Description).NotEmpty().MinimumLength(5);
    }

}