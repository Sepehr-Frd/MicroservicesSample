using RedditMockup.Common.Dtos;

namespace RedditMockup.ExternalService.RabbitMQService.Contracts;

public interface IMessageBusClient : IDisposable
{
    void PublishNewQuestion(QuestionPublishedDto questionPublishedDto);
}