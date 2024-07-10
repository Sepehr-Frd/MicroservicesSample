using FluentValidation;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Common.Validations;

public class QuestionVoteValidator : AbstractValidator<QuestionVote>
{
    public QuestionVoteValidator() =>
        RuleFor(x => x.Kind).NotNull();
}