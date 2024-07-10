using FluentValidation;
using ToDoListManager.Common.Dtos;

namespace ToDoListManager.Common.Validations;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(20).MinimumLength(8);

        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}