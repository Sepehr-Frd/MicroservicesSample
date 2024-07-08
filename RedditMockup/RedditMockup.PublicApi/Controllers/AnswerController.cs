using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedditMockup.Business.EntityBusinesses;
using RedditMockup.Business.Contracts;
using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;

namespace RedditMockup.PublicApi.Controllers;

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
