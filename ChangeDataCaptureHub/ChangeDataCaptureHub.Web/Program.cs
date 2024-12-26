using ChangeDataCaptureHub.ExternalService.ToDoListManager.ToDoListManagerGrpcService;
using ChangeDataCaptureHub.Web;
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