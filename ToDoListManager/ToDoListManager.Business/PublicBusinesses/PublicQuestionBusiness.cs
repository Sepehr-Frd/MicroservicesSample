using Mapster;
using ToDoListManager.Business.Base;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Business.DomainEntityBusinesses;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.PublicBusinesses;

public class PublicQuestionBusiness : PublicBaseBusiness<Question, QuestionDto>
{
    // [Fields]

    private readonly QuestionBusiness _questionBusiness;

    // [Constructor]

    public PublicQuestionBusiness(IBaseBusiness<Question, QuestionDto> questionBusiness) : base(questionBusiness)
    {
        _questionBusiness = (QuestionBusiness)questionBusiness;
    }

    // [Methods]

    public async Task<CustomResponse<List<AnswerDto>>> GetAnswersByQuestionGuidAsync(Guid questionGuid, CancellationToken cancellationToken = default)
    {
        var answersResponse = await _questionBusiness.GetAnswersByQuestionGuidAsync(questionGuid, cancellationToken);

        if (!answersResponse.IsSuccess)
        {
            return CustomResponse<List<AnswerDto>>.CreateUnsuccessfulResponse(answersResponse.HttpStatusCode, answersResponse.Message);
        }

        var answerDtos = answersResponse.Data.Adapt<List<AnswerDto>>();

        return CustomResponse<List<AnswerDto>>.CreateSuccessfulResponse(answerDtos);
    }

    public async Task<CustomResponse<List<VoteDto>>> GetVotesByQuestionGuidAsync(Guid questionGuid, CancellationToken cancellationToken = default)
    {
        var votesResponse = await _questionBusiness.GetVotesByQuestionGuidAsync(questionGuid, cancellationToken);

        if (!votesResponse.IsSuccess)
        {
            return CustomResponse<List<VoteDto>>.CreateUnsuccessfulResponse(votesResponse.HttpStatusCode, votesResponse.Message);
        }

        var voteDtos = votesResponse.Data.Adapt<List<VoteDto>>();

        return CustomResponse<List<VoteDto>>.CreateSuccessfulResponse(voteDtos);
    }

    public async Task<CustomResponse> SubmitVoteAsync(Guid questionGuid, bool kind, CancellationToken cancellationToken = default) =>
        await _questionBusiness.SubmitVoteAsync(questionGuid, kind, cancellationToken);

}