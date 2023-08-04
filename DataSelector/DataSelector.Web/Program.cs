using DataSelector.ExternalService.RedditMockup.RedditMockupGrpcService;
using DataSelector.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .InjectDatabaseSettings(builder.Configuration)
    .InjectRepositories()
    .InjectBusinesses()
    .InjectControllers()
    .InjectServices()
    .InjectAutoMapper()
    .InjectExternalServices()
    .InjectMessageBusSubscriber();

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

using IServiceScope serviceScope = app.Services.CreateScope();

var serviceScopeFactory = serviceScope.ServiceProvider.GetRequiredService<IServiceScopeFactory>();

await PrepareDatabase.PreparePopulationAsync(serviceScopeFactory);

app.Run();
