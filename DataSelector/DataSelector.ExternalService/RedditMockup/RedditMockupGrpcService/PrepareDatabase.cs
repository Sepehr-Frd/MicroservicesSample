using AutoMapper;
using DataSelector.DataAccess;
using DataSelector.Model.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DataSelector.ExternalService.RedditMockup.RedditMockupGrpcService;

public static class PrepareDatabase
{
    public static async Task PreparePopulationAsync(IServiceScopeFactory serviceScopeFactory)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();

        var grpcClient = serviceScope.ServiceProvider.GetRequiredService<IRedditMockupDataClient>();

        var questionDtos = grpcClient.ReturnAllQuestions();

        if (questionDtos is null)
        {
            Console.WriteLine($"No additional questions found, returning from {nameof(PreparePopulationAsync)}");

            return;
        }

        var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();

        var questions = mapper.Map<IEnumerable<QuestionDocument>>(questionDtos);

        var questionRepository = serviceScope.ServiceProvider.GetRequiredService<IBaseRepository<QuestionDocument>>();

        await SeedDataAsync(questionRepository, questions);

    }

    private static async Task SeedDataAsync(IBaseRepository<QuestionDocument> questionRepository, IEnumerable<QuestionDocument> questions)
    {
        await questionRepository.CreateManyAsync(questions);
    }
}