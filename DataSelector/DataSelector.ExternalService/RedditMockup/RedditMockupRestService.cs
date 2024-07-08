using DataSelector.Common.Dtos;
using DataSelector.Model.Models;
using Mapster;
using Newtonsoft.Json;
using RestSharp;

namespace DataSelector.ExternalService.RedditMockup;

public static class RedditMockupRestService
{
    private const string BaseAddress = "http://reddit-mockup-clusterip-service:80/PublicApi";

    public async static Task<List<QuestionDocument>?> GetQuestionsAsync(CancellationToken cancellationToken = default)
    {
        var restClient = new RestClient();

        var restRequest = new RestRequest($"{BaseAddress}/Question")
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        var restResponse = await restClient.ExecuteGetAsync(restRequest, cancellationToken);

        var deserializedResponse = await Task.Factory
            .StartNew(() =>
                    JsonConvert.DeserializeObject<RedditMockupResponseDto<List<QuestionResponseDto>>>(
                        restResponse.Content ?? ""),
                cancellationToken);

        var questions = deserializedResponse?.Data.Adapt<List<QuestionDocument>>();

        return questions;
    }
}