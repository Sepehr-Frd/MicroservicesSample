using ChangeDataCaptureHub.Common.Dtos;
using Grpc.Net.Client;
using Mapster;
using Microsoft.Extensions.Configuration;
using ToDoListManager;

namespace ChangeDataCaptureHub.ExternalService.ToDoListManager.ToDoListManagerGrpcService;

public class ToDoListManagerDataClient(IConfiguration configuration) : IToDoListManagerDataClient
{
    public IEnumerable<ToDoItemDto>? ReturnAllToDoItems()
    {
        var grpcAddress = configuration.GetValue<string>("ToDoListManagerGrpc");

        if (grpcAddress is null)
        {
            Console.WriteLine("ToDoListManagerGrpc address in appsettings is null or invalid!");

            return null;
        }

        var channel = GrpcChannel.ForAddress(grpcAddress);

        var client = new ToDoListManagerGrpc.ToDoListManagerGrpcClient(channel);

        var request = new GetAllRequest();

        try
        {
            var grpcResponse = client.GetAllToDoItems(request);

            return grpcResponse.ToDoItem.Adapt<IEnumerable<ToDoItemDto>>();
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Could not call GRPC Server {exception.Message}");

            return null;
        }
    }
}