using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class QuestionRepository : BaseRepository<Question>
{
    #region [Fields]

    private readonly RedditMockupContext _context;

    #endregion

    #region [Constructor]

    public QuestionRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) : base(context, sieveProcessor) =>
        _context = context;

    #endregion

    #region [Methods]

    public async Task<QuestionVote?> SubmitVoteAsync(QuestionVote questionVote, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _context.QuestionVotes!.AddAsync(questionVote, cancellationToken);

        return entityEntry.Entity;
    }

    #endregion
}