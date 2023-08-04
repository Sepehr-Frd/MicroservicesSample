using DataSelector.Business.Businesses;
using DataSelector.ExternalService.RedditMockup;
using DataSelector.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataSelector.Api.Controllers;

public class QuestionController : BaseController<QuestionDocument>
{
    private readonly QuestionBusiness _questionBusiness;

    private readonly RedditMockupRestService _redditMockupService;

    public QuestionController(QuestionBusiness questionBusiness, RedditMockupRestService redditMockupService) : base(questionBusiness)
    {
        _questionBusiness = questionBusiness;

        _redditMockupService = redditMockupService;
    }

    [HttpGet]
    public async Task<IActionResult> ImportQuestionsAsync(CancellationToken cancellationToken)
    {
        var questions = await _redditMockupService.GetQuestionsAsync(cancellationToken);

        if (questions is null || questions.Count == 0)
        {
            return NoContent();
        }

        await _questionBusiness.CreateManyAsync(questions, cancellationToken);

        return Ok();
    }

}

