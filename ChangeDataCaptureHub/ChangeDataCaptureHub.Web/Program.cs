using ChangeDataCaptureHub.ExternalService.ToDoListManager.ToDoListManagerGrpcService;
using ChangeDataCaptureHub.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddOpenApi()
    .InjectDatabaseSettings(builder.Configuration)
    .InjectRepositories()
    .InjectBusinesses()
    .InjectControllers()
    .InjectExternalServices()
    .InjectMessageBusSubscriber();

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

await builder.Services.InjectRabbitMqAsync(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

using var serviceScope = app.Services.CreateScope();

var serviceScopeFactory = serviceScope.ServiceProvider.GetRequiredService<IServiceScopeFactory>();

await SynchronizeDatabase.FetchNewEntitiesAsync(serviceScopeFactory);

app.Run();