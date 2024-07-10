using FluentValidation;
using ToDoListManager.Common.Dtos;

namespace ToDoListManager.Common.Validations;

public class ProfileDtoValidator : AbstractValidator<ProfileDto>
{
    public ProfileDtoValidator()
    {
        RuleFor(x => x.Bio).MaximumLength(40);

        RuleFor(x => x.Email).EmailAddress();
    }
}