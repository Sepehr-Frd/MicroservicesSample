using FluentValidation;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Validations;

public class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator() =>
        RuleFor(x => x.Title).NotEmpty().MaximumLength(15);
}