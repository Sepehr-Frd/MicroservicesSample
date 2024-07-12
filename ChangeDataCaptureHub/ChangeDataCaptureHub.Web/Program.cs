using ChangeDataCaptureHub.ExternalService.ToDoListManager.ToDoListManagerGrpcService;
using ChangeDataCaptureHub.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .InjectDatabaseSettings(builder.Configuration)
    .InjectRepositories()
    .InjectBusinesses()
    .InjectControllers()
    .InjectExternalServices()
    .InjectMessageBusSubscriber();

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

using var serviceScope = app.Services.CreateScope();

var serviceScopeFactory = serviceScope.ServiceProvider.GetRequiredService<IServiceScopeFactory>();

await PrepareDatabase.PreparePopulationAsync(serviceScopeFactory);

app.Run();