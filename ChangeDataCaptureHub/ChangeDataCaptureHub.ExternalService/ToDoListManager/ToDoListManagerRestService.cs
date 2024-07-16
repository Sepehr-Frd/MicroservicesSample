using ChangeDataCaptureHub.Common.Dtos;
using ChangeDataCaptureHub.Model.Models;
using Mapster;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace ChangeDataCaptureHub.ExternalService.ToDoListManager;

public class ToDoListManagerRestService
{
    private readonly string? _baseAddress;

    public ToDoListManagerRestService(IConfiguration configuration)
    {
        _baseAddress = configuration.GetValue<string>("ToDoListManagerRestBaseAddress");
    }

    public async Task<List<ToDoItemDocument>?> GetToDoItemsAsync(CancellationToken cancellationToken = default)
    {
        var restClient = new RestClient();

        var restRequest = new RestRequest($"{_baseAddress}/to-do-items/all-to-do-items")
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        var restResponse = await restClient.ExecuteGetAsync(restRequest, cancellationToken);

        var deserializedResponse = JsonConvert.DeserializeObject<List<ToDoItemDto>>(restResponse.Content ?? string.Empty);

        var toDoItems = deserializedResponse?.Adapt<List<ToDoItemDocument>>();

        return toDoItems;
    }
}