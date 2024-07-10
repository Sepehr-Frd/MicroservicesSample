using Microsoft.AspNetCore.Mvc;
using ToDoListManager.Api.Base;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Business.DomainEntityBusinesses;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Api.Controllers;

[Route("answers")]
public class AnswerController : BaseController<Answer, AnswerDto>
{
    // [Fields]

    private readonly AnswerBusiness _business;

    // [Constructor]

    public AnswerController(IBaseBusiness<Answer, AnswerDto> business) : base(business) =>
        _business = (AnswerBusiness)business;

    // [Methods]

    [HttpGet]
    [Route("{guid:guid}/votes")]
    public async Task<CustomResponse<List<AnswerVote>>> GetVotesByAnswerGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _business.GetVotesByAnswerGuidAsync(guid, cancellationToken);

    [HttpPost]
    [Route("{answerGuid:guid}/votes")]
    public async Task<CustomResponse> SubmitVoteAsync([FromRoute] Guid answerGuid, [FromBody] bool kind, CancellationToken cancellationToken) =>
        await _business.SubmitVoteAsync(answerGuid, kind, cancellationToken);

}