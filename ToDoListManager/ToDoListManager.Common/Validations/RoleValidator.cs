using FluentValidation;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Common.Validations;

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator() =>
        RuleFor(x => x.Title).NotEmpty().MaximumLength(15);
}