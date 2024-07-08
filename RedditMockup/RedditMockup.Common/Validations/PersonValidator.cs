using FluentValidation;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Validations;

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(20);

        RuleFor(x => x.LastName).NotEmpty().MaximumLength(20);
    }
}