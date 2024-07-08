using RedditMockup.DataAccess.Base;
using RedditMockup.DataAccess.Context;
using RedditMockup.Model.Entities;
using Sieve.Services;

namespace RedditMockup.DataAccess.Repositories;

public class AnswerRepository : BaseRepository<Answer>
{
    // [Fields]

    private readonly RedditMockupContext _context;

    // [Constructor]

    public AnswerRepository(RedditMockupContext context, ISieveProcessor sieveProcessor) :
        base(context, sieveProcessor) =>
        _context = context;

    // [Methods]

    public async Task<AnswerVote?> SubmitVoteAsync(AnswerVote answerVote, CancellationToken cancellationToken = default)
    {
        var entityEntry = await _context.AnswerVotes!.AddAsync(answerVote, cancellationToken);

        return entityEntry.Entity;
    }

}