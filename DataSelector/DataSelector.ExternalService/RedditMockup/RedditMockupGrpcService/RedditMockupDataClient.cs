using AutoMapper;
using DataSelector.Common.Dtos;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using RedditMockup;

namespace DataSelector.ExternalService.RedditMockup.RedditMockupGrpcService;

public class RedditMockupDataClient(IConfiguration configuration, IMapperBase mapper) : IRedditMockupDataClient
{
    public IEnumerable<QuestionResponseDto>? ReturnAllQuestions()
    {
        var grpcAddress = configuration.GetValue<string>("RedditMockupGrpc");

        if (grpcAddress is null)
        {
            Console.WriteLine("RedditMockupGrpc address in appsettings is null or invalid!");

            return null;
        }

        var channel = GrpcChannel.ForAddress(grpcAddress);

        var client = new RedditMockupGrpc.RedditMockupGrpcClient(channel);

        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllQuestions(request);
            return mapper.Map<IEnumerable<QuestionResponseDto>>(reply.Question);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Could not call GRPC Server {exception.Message}");

            return null;
        }
    }
}