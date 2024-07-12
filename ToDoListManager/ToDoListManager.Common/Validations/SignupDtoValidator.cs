using FluentValidation;
using ToDoListManager.Common.Constants;
using ToDoListManager.Common.Dtos;

namespace ToDoListManager.Common.Validations;

public class SignupDtoValidator : AbstractValidator<SignupDto>
{
    public SignupDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Username)
            .NotEmpty()
            .Matches(RegexPatternConstants.UsernamePattern);

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatternConstants.PasswordPattern);

        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(20);

        RuleFor(x => x.LastName).MaximumLength(20);
    }
}