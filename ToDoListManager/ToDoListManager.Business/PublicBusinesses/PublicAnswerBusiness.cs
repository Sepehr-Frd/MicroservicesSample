using Mapster;
using ToDoListManager.Business.Base;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Business.DomainEntityBusinesses;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.PublicBusinesses;

public class PublicAnswerBusiness : PublicBaseBusiness<Answer, AnswerDto>
{
    // [Fields]

    private readonly AnswerBusiness _answerBusiness;

    // [Constructor]

    public PublicAnswerBusiness(IBaseBusiness<Answer, AnswerDto> answerBusiness) : base(answerBusiness)
    {
        _answerBusiness = (AnswerBusiness)answerBusiness;
    }

    // [Methods]

    public async Task<CustomResponse<List<VoteDto>>> GetVotesByAnswerGuidAsync(Guid answerGuid, CancellationToken cancellationToken = default)
    {
        var votesResponse = await _answerBusiness.GetVotesByAnswerGuidAsync(answerGuid, cancellationToken);

        if (!votesResponse.IsSuccess)
        {
            return CustomResponse<List<VoteDto>>.CreateUnsuccessfulResponse(votesResponse.HttpStatusCode, votesResponse.Message);
        }

        var voteDtos = votesResponse.Data.Adapt<List<VoteDto>>();

        return CustomResponse<List<VoteDto>>.CreateSuccessfulResponse(voteDtos);
    }

    public async Task<CustomResponse> SubmitVoteAsync(Guid answerGuid, bool kind, CancellationToken cancellationToken = default) =>
        await _answerBusiness.SubmitVoteAsync(answerGuid, kind, cancellationToken);

}