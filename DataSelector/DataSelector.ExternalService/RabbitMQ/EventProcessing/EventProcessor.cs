using System.Text.Json;
using DataSelector.Common.Dtos;
using DataSelector.DataAccess;
using DataSelector.Model.Models;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace DataSelector.ExternalService.RabbitMQ.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task ProcessEventAsync(string message, CancellationToken cancellationToken = default)
    {
        var eventType = DetermineEventType(message);

        if (eventType is EventType.QuestionPublished)
        {
            await AddQuestionAsync(message, cancellationToken);
        }
    }

    private static EventType DetermineEventType(string notificationMessage)
    {
        var eventDto = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        return eventDto?.Event switch
        {
            "Question_Published" => EventType.QuestionPublished,
            _ => EventType.Undetermined
        };
    }

    private async Task AddQuestionAsync(string questionPublishedMessage, CancellationToken cancellationToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var repository = scope.ServiceProvider.GetRequiredService<IBaseRepository<QuestionDocument>>();

        var questionPublishedDto = JsonSerializer.Deserialize<QuestionPublishedDto>(questionPublishedMessage);

        try
        {
            var questionDocument = questionPublishedDto.Adapt<QuestionDocument>();
            await repository.CreateOneAsync(questionDocument, cancellationToken);
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Could not add question document to database due to an exception: {exception.Message}");
        }
    }
}