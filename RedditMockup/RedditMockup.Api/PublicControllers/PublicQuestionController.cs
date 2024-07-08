using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedditMockup.Api.Base;
using RedditMockup.Business.Contracts;
using RedditMockup.Business.PublicBusinesses;
using RedditMockup.Common.Dtos;
using RedditMockup.ExternalService.RabbitMQService.Contracts;
using RedditMockup.Model.Entities;

namespace RedditMockup.Api.PublicControllers;

[Route("public/questions")]
public class PublicQuestionController : PublicBaseController<Question, QuestionDto>
{
    // [Fields]

    private readonly PublicQuestionBusiness _publicQuestionBusiness;

    private readonly IMessageBusClient _messageBusClient;

    // [Constructor]

    public PublicQuestionController(IPublicBaseBusiness<QuestionDto> questionDtoBaseBusiness, IMessageBusClient messageBusClient) : base(questionDtoBaseBusiness)
    {
        _publicQuestionBusiness = (PublicQuestionBusiness)questionDtoBaseBusiness;

        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    [Route("guid/{guid:guid}/answers")]
    public async Task<CustomResponse<List<AnswerDto>>> GetAnswersByQuestionGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _publicQuestionBusiness.GetAnswersByQuestionGuidAsync(guid, cancellationToken);

    [HttpGet]
    [Route("guid/{guid:guid}/votes")]
    public async Task<CustomResponse<List<VoteDto>>> GetVotesByQuestionGuidAsync([FromRoute] Guid guid, CancellationToken cancellationToken) =>
        await _publicQuestionBusiness.GetVotesByQuestionGuidAsync(guid, cancellationToken);

    [Authorize]
    [HttpPost]
    [Route("guid/{guid:guid}/votes")]
    public async Task<CustomResponse> SubmitVoteAsync([FromRoute] Guid guid, [FromBody] bool kind, CancellationToken cancellationToken) =>
        await _publicQuestionBusiness.SubmitVoteAsync(guid, kind, cancellationToken);

    // ------------------------------------------------------------------------>

    [HttpPost]
    [Route("test-rabbitmq")]
    public async Task CreateAndPublishAsync([FromBody] QuestionDto questionDto, CancellationToken cancellationToken)
    {
        await CreateAsync(questionDto, cancellationToken);

        var questionPublishedDto = new QuestionPublishedDto
        {
            Title = questionDto.Title,
            Description = questionDto.Description,
            Event = "Question_Published"
        };

        _messageBusClient.PublishNewQuestion(questionPublishedDto);
    }

    // <-----------------------------------------------------------------------
}