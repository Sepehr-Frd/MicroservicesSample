using ChangeDataCaptureHub.DataAccess;
using ChangeDataCaptureHub.Model.Models;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace ChangeDataCaptureHub.ExternalService.ToDoListManager.ToDoListManagerGrpcService;

public static class PrepareDatabase
{
    public async static Task PreparePopulationAsync(IServiceScopeFactory serviceScopeFactory)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();

        var grpcClient = serviceScope.ServiceProvider.GetRequiredService<IToDoListManagerDataClient>();

        var toDoItemDtos = grpcClient.ReturnAllToDoItems();

        if (toDoItemDtos is null)
        {
            Console.WriteLine($"No additional toDoItems found, returning from {nameof(PreparePopulationAsync)}");

            return;
        }

        var toDoItems = toDoItemDtos.Adapt<IEnumerable<ToDoItemDocument>>();

        var toDoItemRepository = serviceScope.ServiceProvider.GetRequiredService<IBaseRepository<ToDoItemDocument>>();

        await SeedDataAsync(toDoItemRepository, toDoItems);
    }

    private async static Task SeedDataAsync(IBaseRepository<ToDoItemDocument> toDoItemRepository, IEnumerable<ToDoItemDocument> toDoItems)
    {
        await toDoItemRepository.CreateManyAsync(toDoItems);
    }
}