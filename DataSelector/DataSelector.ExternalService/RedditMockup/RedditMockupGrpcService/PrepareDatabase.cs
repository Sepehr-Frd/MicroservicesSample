using DataSelector.DataAccess;
using DataSelector.Model.Models;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace DataSelector.ExternalService.RedditMockup.RedditMockupGrpcService;

public static class PrepareDatabase
{
    public async static Task PreparePopulationAsync(IServiceScopeFactory serviceScopeFactory)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();

        var grpcClient = serviceScope.ServiceProvider.GetRequiredService<IRedditMockupDataClient>();

        var questionDtos = grpcClient.ReturnAllQuestions();

        if (questionDtos is null)
        {
            Console.WriteLine($"No additional questions found, returning from {nameof(PreparePopulationAsync)}");

            return;
        }

        var questions = questionDtos.Adapt<IEnumerable<QuestionDocument>>();

        var questionRepository = serviceScope.ServiceProvider.GetRequiredService<IBaseRepository<QuestionDocument>>();

        await SeedDataAsync(questionRepository, questions);
    }

    private async static Task SeedDataAsync(IBaseRepository<QuestionDocument> questionRepository, IEnumerable<QuestionDocument> questions)
    {
        await questionRepository.CreateManyAsync(questions);
    }
}