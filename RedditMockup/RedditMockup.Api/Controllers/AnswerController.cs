using Microsoft.AspNetCore.Mvc;
using RedditMockup.Api.Base;
using RedditMockup.Business.Contracts;
using RedditMockup.Business.DomainEntityBusinesses;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.Api.Controllers;

[Route("answers")]
public class AnswerController : BaseController<Answer, AnswerDto>
{
    #region [Fields]

    private readonly AnswerBusiness _business;

    #endregion

    #region [Constructor]

    public AnswerController(IBaseBusiness<Answer, AnswerDto> business) : base(business) =>
        _business = (AnswerBusiness)business;

    #endregion

    #region [Methods]

    [HttpGet]
    [Route("{guid}/votes")]
    public async Task<CustomResponse<List<AnswerVote>>> GetVotesByAnswerGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _business.GetVotesByAnswerGuidAsync(guid, cancellationToken);

    [HttpPost]
    [Route("{guid}/votes")]
    public async Task<CustomResponse> SubmitVoteAsync([FromRoute] Guid answerGuid, [FromBody] bool kind, CancellationToken cancellationToken) =>
        await _business.SubmitVoteAsync(answerGuid, kind, cancellationToken);

    #endregion
}