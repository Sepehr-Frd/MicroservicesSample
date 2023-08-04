using RedditMockup.DataAccess.Repositories;

namespace RedditMockup.DataAccess.Contracts;

public interface IUnitOfWork
{
    #region [Properties]

    AnswerRepository? AnswerRepository { get; }

    PersonRepository? PersonRepository { get; }

    QuestionRepository? QuestionRepository { get; }

    RoleRepository? RoleRepository { get; }

    UserRepository? UserRepository { get; }

    #endregion

    #region [Methods]

    Task<int> CommitAsync(CancellationToken cancellationToken);

    #endregion
}