using Grpc.Core;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using Sieve.Models;

namespace RedditMockup.Service.Grpc;

public class GrpcService : RedditMockupGrpc.RedditMockupGrpcBase
{
    private readonly QuestionRepository? _questionRepository;

    public GrpcService(IServiceScopeFactory serviceScopeFactory)
    {
        var unitOfWork = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

        _questionRepository = (QuestionRepository)unitOfWork.QuestionRepository!;
    }

    public async override Task<GrpcResponse?> GetAllQuestions(GetAllRequest request, ServerCallContext context)
    {
        if (_questionRepository is null)
        {
            return null;
        }

        var response = new GrpcResponse();

        var questions = await _questionRepository.GetAllAsync(new SieveModel());

        var questionDtos = questions.Adapt<List<GrpcQuestionModel>>();

        response.Question.AddRange(questionDtos);

        return response;
    }
}