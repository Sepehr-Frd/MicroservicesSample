using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListManager.Api.Base;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Business.PublicBusinesses;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Api.PublicControllers;

[Route("public/answers")]
public class PublicAnswerController : PublicBaseController<Answer, AnswerDto>
{
    // [Fields]

    private readonly PublicAnswerBusiness _publicAnswerBusiness;

    // [Constructor]

    public PublicAnswerController(IPublicBaseBusiness<AnswerDto> publicBaseBusiness) : base(publicBaseBusiness) =>
        _publicAnswerBusiness = (PublicAnswerBusiness)publicBaseBusiness;

    // [Methods]

    [HttpGet]
    [Route("guid/{guid:guid}/votes")]
    public async Task<CustomResponse<List<VoteDto>>> GetVotesByAnswerGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _publicAnswerBusiness.GetVotesByAnswerGuidAsync(guid, cancellationToken);

    [Authorize]
    [HttpPost]
    [Route("guid/{guid:guid}/votes")]
    public async Task<CustomResponse> SubmitVoteAsync([FromRoute] Guid guid, [FromBody] bool kind, CancellationToken cancellationToken) =>
        await _publicAnswerBusiness.SubmitVoteAsync(guid, kind, cancellationToken);

}