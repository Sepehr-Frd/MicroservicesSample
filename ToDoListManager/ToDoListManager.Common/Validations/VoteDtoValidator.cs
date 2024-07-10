using FluentValidation;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Common.Validations;

public class AnswerVoteValidator : AbstractValidator<AnswerVote>
{
    public AnswerVoteValidator() =>
        RuleFor(x => x.Kind).NotNull();
}