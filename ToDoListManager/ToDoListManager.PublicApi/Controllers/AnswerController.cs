using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoListManager.Business.EntityBusinesses;
using ToDoListManager.Business.Contracts;
using ToDoListManager.Common.Dtos;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.PublicApi.Controllers;

public class AnswerController : ControllerBase
{

    private readonly AnswerBusiness _answerBusiness;

    public AnswerController(IBaseBusiness<Answer> answerBusiness) => 
        _answerBusiness = (AnswerBusiness)answerBusiness;

    [HttpGet]
    [Route("AnswersByQuestionId")]
    public async Task<CustomResponse?> GetAnswersByQuestionIdAsync(int questionId, CancellationToken cancellationToken) =>
        await _answerBusiness.LoadAnswersByQuestionIdAsync(questionId, cancellationToken);

    [Authorize]
    [HttpPost]
    [Route("SubmitVote")]
    public async Task<CustomResponse?> SubmitVoteAsync(int answerId, bool kind, CancellationToken cancellationToken) =>
    await _answerBusiness.SubmitVoteAsync(answerId, kind, cancellationToken);

    [HttpGet]
    [Route("AnswerVotes")]
    public async Task<CustomResponse?> GetVotesAsync(int answerId, CancellationToken cancellationToken) =>
        await _answerBusiness.LoadVotesAsync(answerId, cancellationToken);

}
