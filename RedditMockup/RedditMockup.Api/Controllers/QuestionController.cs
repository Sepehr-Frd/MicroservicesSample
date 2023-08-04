using Microsoft.AspNetCore.Mvc;
using RedditMockup.Api.Base;
using RedditMockup.Business.Contracts;
using RedditMockup.Business.DomainEntityBusinesses;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Api.Controllers;

[Route("questions")]
public class QuestionController : BaseController<Question, QuestionDto>
{
    private readonly QuestionBusiness _business;

    public QuestionController(IBaseBusiness<Question, QuestionDto> business) : base(business)
    {
        _business = (QuestionBusiness)business;
    }

    [HttpGet]
    [Route("{guid}/answers")]
    public async Task<CustomResponse<List<Answer>>> GetAnswersByQuestionGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _business.GetAnswersByQuestionGuidAsync(guid, cancellationToken);


    [HttpGet]
    [Route("{guid}/votes")]
    public async Task<CustomResponse<List<QuestionVote>>> GetVotesByQuestionGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _business.GetVotesByQuestionGuidAsync(guid, cancellationToken);


    [HttpPost]
    [Route("{guid}/votes")]
    public async Task<CustomResponse> SubmitVoteAsync([FromRoute] Guid guid, [FromBody] bool kind, CancellationToken cancellationToken) =>
        await _business.SubmitVoteAsync(guid, kind, cancellationToken);
}