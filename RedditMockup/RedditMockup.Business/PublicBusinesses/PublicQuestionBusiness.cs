using AutoMapper;
using RedditMockup.Business.Base;
using RedditMockup.Business.Contracts;
using RedditMockup.Business.DomainEntityBusinesses;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Business.PublicBusinesses;

public class PublicQuestionBusiness : PublicBaseBusiness<Question, QuestionDto>
{
    #region [Fields]

    private readonly QuestionBusiness _questionBusiness;

    private readonly IMapper _mapper;

    #endregion

    #region [Constructor]

    public PublicQuestionBusiness(IBaseBusiness<Question, QuestionDto> questionBusiness, IMapper mapper) : base(questionBusiness, mapper)
    {
        _questionBusiness = (QuestionBusiness)questionBusiness;

        _mapper = mapper;
    }

    #endregion

    #region [Methods]

    public async Task<CustomResponse<List<AnswerDto>>> GetAnswersByQuestionGuidAsync(Guid questionGuid, CancellationToken cancellationToken = default)
    {
        var answersResponse = await _questionBusiness.GetAnswersByQuestionGuidAsync(questionGuid, cancellationToken);

        if (!answersResponse.IsSuccess)
        {
            return CustomResponse<List<AnswerDto>>.CreateUnsuccessfulResponse(answersResponse.HttpStatusCode, answersResponse.Message);
        }

        var answerDtos = _mapper.Map<List<AnswerDto>>(answersResponse.Data);

        return CustomResponse<List<AnswerDto>>.CreateSuccessfulResponse(answerDtos);
    }

    public async Task<CustomResponse<List<VoteDto>>> GetVotesByQuestionGuidAsync(Guid questionGuid, CancellationToken cancellationToken = default)
    {
        var votesResponse = await _questionBusiness.GetVotesByQuestionGuidAsync(questionGuid, cancellationToken);

        if (!votesResponse.IsSuccess)
        {
            return CustomResponse<List<VoteDto>>.CreateUnsuccessfulResponse(votesResponse.HttpStatusCode, votesResponse.Message);
        }

        var voteDtos = _mapper.Map<List<VoteDto>>(votesResponse.Data);

        return CustomResponse<List<VoteDto>>.CreateSuccessfulResponse(voteDtos);
    }

    public async Task<CustomResponse> SubmitVoteAsync(Guid questionGuid, bool kind, CancellationToken cancellationToken = default) =>
        await _questionBusiness.SubmitVoteAsync(questionGuid, kind, cancellationToken);

    #endregion
}