using FluentValidation;
using RedditMockup.Common.Dtos;

namespace RedditMockup.Common.Validations;

public class AnswerDtoValidator : AbstractValidator<AnswerDto>
{
    public AnswerDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(5);

        RuleFor(x => x.Description).NotEmpty().MinimumLength(5);
    }

}