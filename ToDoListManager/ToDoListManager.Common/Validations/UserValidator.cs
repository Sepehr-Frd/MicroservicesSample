using FluentValidation;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Common.Validations;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(20).MinimumLength(8);

        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}