using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class AnswerRepository : BaseRepository<Answer>
{
    #region [Fields]

    private readonly RedditMockupContext _context;

    #endregion

    #region [Constructor]

    public AnswerRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) :
        base(context, sieveProcessor) =>
        _context = context;

    #endregion

    #region [Methods]

    public async Task<AnswerVote?> SubmitVoteAsync(AnswerVote answerVote, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _context.AnswerVotes!.AddAsync(answerVote, cancellationToken);

        return entityEntry.Entity;
    }

    #endregion

}