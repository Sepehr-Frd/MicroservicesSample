using System.Net;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using ToDoListManager.Business.Base;
using ToDoListManager.Common.Dtos;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.DataAccess.Repositories;
using ToDoListManager.Model.Entities;

namespace ToDoListManager.Business.DomainEntityBusinesses;

public class AnswerBusiness : BaseBusiness<Answer, AnswerDto>
{
    // [Fields]

    private readonly IUnitOfWork _unitOfWork;
    private readonly AnswerRepository _answerRepository;

    // [Constructor]

    public AnswerBusiness(IUnitOfWork unitOfWork, IBaseRepository<Answer> repository) : base(unitOfWork, repository)
    {
        _unitOfWork = unitOfWork;
        _answerRepository = (AnswerRepository)unitOfWork.AnswerRepository!;
    }

    // [Methods]

    public async override Task<Answer?> CreateAsync(AnswerDto userDto, CancellationToken cancellationToken = default)
    {
        var answer = userDto.Adapt<Answer>();

        var user = await _unitOfWork.UserRepository!.GetByGuidAsync(userDto.UserGuid, null, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var question = await _unitOfWork.QuestionRepository!.GetByGuidAsync(userDto.QuestionGuid, null, cancellationToken);

        if (question is null)
        {
            return null;
        }

        answer.UserId = user.Id;

        answer.QuestionId = question.Id;

        return await CreateAsync(answer, cancellationToken);
    }

    public async override Task<Answer?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await _answerRepository.GetByIdAsync(id,
            answers => answers.Include(answer => answer.User)
                .Include(answer => answer.Question),
            cancellationToken);

    public async override Task<Answer?> GetByGuidAsync(Guid guid, CancellationToken cancellationToken = default) =>
        await _answerRepository.GetByGuidAsync(guid,
            answers => answers.Include(answer => answer.User)
                .Include(answer => answer.Question),
            cancellationToken);

    public async override Task<List<Answer>?> GetAllAsync(SieveModel sieveModel, CancellationToken cancellationToken = default) =>
        await _answerRepository.GetAllAsync(sieveModel,
            answers => answers.Include(answer => answer.User)
                .Include(answer => answer.Question),
            cancellationToken);

    public async Task<CustomResponse> SubmitVoteAsync(Guid answerGuid, bool kind, CancellationToken cancellationToken = default)
    {
        var answer = await _answerRepository.GetByGuidAsync(answerGuid,
            answers => answers.Include(answer => answer.User)
                .Include(answer => answer.Votes),
            cancellationToken);

        if (answer is null)
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.NotFound, $"No answer found with guid of {answerGuid}");
        }

        if (answer.User is null)
        {
            return CustomResponse.CreateUnsuccessfulResponse(HttpStatusCode.InternalServerError);
        }

        if (kind)
        {
            answer.User.Score += 1;
        }

        var vote = new AnswerVote
        {
            Kind = kind,
            AnswerId = answer.Id
        };

        await _answerRepository.SubmitVoteAsync(vote, cancellationToken);

        await _unitOfWork.CommitAsync(cancellationToken);

        return CustomResponse.CreateSuccessfulResponse($"{(kind ? "Up" : "Down")}vote submitted");
    }

    public async Task<CustomResponse<List<AnswerVote>>> GetVotesByAnswerGuidAsync(Guid answerGuid, CancellationToken cancellationToken = default)
    {
        var answer = await _answerRepository.GetByGuidAsync(answerGuid,
            answers => answers.Include(answer => answer.Votes), cancellationToken);

        if (answer is null)
        {
            return CustomResponse<List<AnswerVote>>.CreateUnsuccessfulResponse(HttpStatusCode.NotFound);
        }

        var votes = answer.Votes!.ToList();

        return CustomResponse<List<AnswerVote>>.CreateSuccessfulResponse(votes);
    }
}