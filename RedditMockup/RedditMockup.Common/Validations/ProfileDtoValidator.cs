using FluentValidation;
using RedditMockup.Common.Dtos;

namespace RedditMockup.Common.Validations;

public class ProfileDtoValidator : AbstractValidator<ProfileDto>
{
    public ProfileDtoValidator()
    {
        RuleFor(x => x.Bio).MaximumLength(40);

        RuleFor(x => x.Email).EmailAddress();
    }
}