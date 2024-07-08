using System.Net;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RedditMockup.Business.Base;
using RedditMockup.Common.Dtos;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using RedditMockup.Model.Entities;
using Sieve.Models;

namespace RedditMockup.Business.DomainEntityBusinesses;

public class QuestionBusiness : BaseBusiness<Question, QuestionDto>
{
    // [Fields]

    private readonly QuestionRepository _questionRepository;

    private readonly IUnitOfWork _unitOfWork;

    // [Constructor]

    public QuestionBusiness(IUnitOfWork unitOfWork) : base(unitOfWork, unitOfWork.QuestionRepository!)
    {
        _questionRepository = (QuestionRepository)unitOfWork.QuestionRepository!;

        _unitOfWork = unitOfWork;
    }

    // [Methods]

    public async override Task<Question?> CreateAsync(QuestionDto userDto, CancellationToken cancellationToken = default)
    {
        var question = userDto.Adapt<Question>();

        var user = await _unitOfWork.UserRepository!.GetByGuidAsync(userDto.UserGuid, null, cancellationToken);

        if (user is null)
        {
            return null;
        }

        question.UserId = user.Id;

        return await CreateAsync(question, cancellationToken);
    }

    public async override Task<Question?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _questionRepository.GetByIdAsync(id,
            questions => questions.Include(question => question.User),
            cancellationToken);

    public async override Task<Question?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default) =>
        await _questionRepository.GetByGuidAsync(guid,
            questions => questions.Include(question => question.User),
            cancellationToken);

    public async override Task<List<Question>?> GetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = default) =>
        await _questionRepository.GetAllAsync(sieveModel,
            questions => questions.Include(question => question.User),
            cancellationToken);

    public async Task<CustomResponse<List<Answer>>> GetAnswersByQuestionGuidAsync(Guid questionGuid, CancellationToken cancellationToken = default)
    {
        var question = await _unitOfWork.QuestionRepository!.GetByGuidAsync(questionGuid, null, cancellationToken);

        if (question is null)
        {
            return CustomResponse<List<Answer>>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound);
        }

        SieveModel sieveModel = new()
        {
            Filters = $"QuestionId=={question.Id}"
        };

        var answers = await _unitOfWork.AnswerRepository!.GetAllAsync(sieveModel, null, cancellationToken);

        if (answers.IsNullOrEmpty())
        {
            return CustomResponse<List<Answer>>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound, $"No answer found with question guid of {questionGuid}");
        }

        return CustomResponse<List<Answer>>.CreateSuccessfulResponse(answers);
    }

    public async Task<CustomResponse<List<QuestionVote>>> GetVotesByQuestionGuidAsync(Guid questionGuid, CancellationToken cancellationToken = default)
    {
        var question = await _questionRepository.GetByGuidAsync(questionGuid,
            questions => questions
                .Include(question => question.Votes)
                .Include(question => question.Answers), cancellationToken);

        if (question is null)
        {
            return CustomResponse<List<QuestionVote>>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound, $"No question found with guid of {questionGuid}");
        }

        var votes = question.Votes!.ToList();

        return CustomResponse<List<QuestionVote>>.CreateSuccessfulResponse(votes);
    }

    public async Task<CustomResponse> SubmitVoteAsync(Guid questionGuid, bool kind, CancellationToken cancellationToken = default)
    {
        var question = await _questionRepository.GetByGuidAsync(questionGuid,
            questions => questions.Include(question => question.User)
                .Include(question => question.Votes),
            cancellationToken);

        if (question is null)
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.NotFound, $"No question found with guid of {questionGuid}");
        }

        if (question.User is null)
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.InternalServerError);
        }

        if (kind)
        {
            question.User.Score += 1;
        }

        var vote = new QuestionVote
        {
            Kind = kind,
            QuestionId = question.Id
        };

        var voteResult = await _questionRepository.SubmitVoteAsync(vote, cancellationToken);

        if (voteResult is null)
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.InternalServerError);
        }

        await _unitOfWork.CommitAsync(cancellationToken);

        return CustomResponse.CreateSuccessfulResponse($"{(kind ? "Up" : "Down")}vote submitted");
    }

}