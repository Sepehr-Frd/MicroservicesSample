using FluentValidation;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Validations;

public class QuestionVoteValidator : AbstractValidator<QuestionVote>
{
    public QuestionVoteValidator() =>
        RuleFor(x => x.Kind).NotNull();
}