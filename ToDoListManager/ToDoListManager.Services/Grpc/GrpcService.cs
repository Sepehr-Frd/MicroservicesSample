using Grpc.Core;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using ToDoListManager.DataAccess.Contracts;
using ToDoListManager.DataAccess.Repositories;

namespace ToDoListManager.Services.Grpc;

public class GrpcService : ToDoListManagerGrpc.ToDoListManagerGrpcBase
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

        var questions = await _questionRepository.GetAllAsync();

        var questionDtos = questions.Adapt<List<GrpcQuestionModel>>();

        response.Question.AddRange(questionDtos);

        return response;
    }
}