using FluentValidation;
using RedditMockup.Model.Entities;

namespace RedditMockup.Common.Validations;

public class AnswerVoteValidator : AbstractValidator<AnswerVote>
{
    public AnswerVoteValidator() =>
        RuleFor(x => x.Kind).NotNull();
}

public class QuestionVoteValidator : AbstractValidator<QuestionVote>
{
    public QuestionVoteValidator() =>
        RuleFor(x => x.Kind).NotNull();
}