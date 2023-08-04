using DataSelector.Common.Dtos;

namespace DataSelector.ExternalService.RedditMockup.RedditMockupGrpcService;

public interface IRedditMockupDataClient
{
    IEnumerable<QuestionResponseDto>? ReturnAllQuestions();
}
