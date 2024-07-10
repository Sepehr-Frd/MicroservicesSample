using Microsoft.AspNetCore.Mvc;
using ToDoListManager.Api.Base;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Business.DomainEntityBusinesses;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Api.Controllers;

[Route("questions")]
public class QuestionController : BaseController<Question, QuestionDto>
{
    private readonly QuestionBusiness _business;

    public QuestionController(IBaseBusiness<Question, QuestionDto> business) : base(business)
    {
        _business = (QuestionBusiness)business;
    }

    [HttpGet]
    [Route("{guid:guid}/answers")]
    public async Task<CustomResponse<List<Answer>>> GetAnswersByQuestionGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _business.GetAnswersByQuestionGuidAsync(guid, cancellationToken);

    [HttpGet]
    [Route("{guid:guid}/votes")]
    public async Task<CustomResponse<List<QuestionVote>>> GetVotesByQuestionGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _business.GetVotesByQuestionGuidAsync(guid, cancellationToken);

    [HttpPost]
    [Route("{guid:guid}/votes")]
    public async Task<CustomResponse> SubmitVoteAsync([FromRoute] Guid guid, [FromBody] bool kind, CancellationToken cancellationToken) =>
        await _business.SubmitVoteAsync(guid, kind, cancellationToken);
}