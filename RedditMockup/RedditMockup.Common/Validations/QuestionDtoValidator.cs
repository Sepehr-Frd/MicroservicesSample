using FluentValidation;
using RedditMockup.Common.Dtos;

namespace RedditMockup.Common.Validations;

public class QuestionDtoValidator : AbstractValidator<QuestionDto>
{
    public QuestionDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MinimumLength(5);

        RuleFor(x => x.Description).NotEmpty().MinimumLength(10);
    }
}