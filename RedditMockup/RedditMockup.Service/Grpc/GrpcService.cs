using AutoMapper;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.DataAccess.Repositories;
using Sieve.Models;

namespace RedditMockup.Service.Grpc;

public class GrpcService : RedditMockupGrpc.RedditMockupGrpcBase
{
    private readonly QuestionRepository? _questionRepository;

    private readonly IMapper _mapper;

    public GrpcService(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        _mapper = mapper;

        var unitOfWork = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

        _questionRepository = unitOfWork.QuestionRepository;
    }

    public override async Task<GrpcResponse?> GetAllQuestions(GetAllRequest request, ServerCallContext context)
    {
        if (_questionRepository is null)
        {
            return null;
        }

        var response = new GrpcResponse();

        var questions = await _questionRepository.GetAllAsync(new SieveModel());

        var questionDtos = _mapper.Map<List<GrpcQuestionModel>>(questions);

        response.Question.AddRange(questionDtos);

        return response;
    }
}