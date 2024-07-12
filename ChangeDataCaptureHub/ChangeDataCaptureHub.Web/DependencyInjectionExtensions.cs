using ChangeDataCaptureHub.Business.Businesses;
using ChangeDataCaptureHub.DataAccess;
using ChangeDataCaptureHub.DataAccess.Repositories;
using ChangeDataCaptureHub.ExternalService.RabbitMQ;
using ChangeDataCaptureHub.ExternalService.RabbitMQ.EventProcessing;
using ChangeDataCaptureHub.ExternalService.ToDoListManager;
using ChangeDataCaptureHub.ExternalService.ToDoListManager.ToDoListManagerGrpcService;
using ChangeDataCaptureHub.Model.Models;

namespace ChangeDataCaptureHub.Web;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection InjectControllers(this IServiceCollection services) => services.AddControllers().Services;

    public static IServiceCollection InjectDatabaseSettings(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));

    public static IServiceCollection InjectRepositories(this IServiceCollection services) =>
        services.AddScoped<IBaseRepository<ToDoItemDocument>, ToDoItemRepository>();

    public static IServiceCollection InjectBusinesses(this IServiceCollection services) =>
        services.AddScoped<ToDoItemBusiness>();

    internal static IServiceCollection InjectExternalServices(this IServiceCollection services) =>
        services
            .AddSingleton<IEventProcessor, EventProcessor>()
            .AddScoped<IToDoListManagerDataClient, ToDoListManagerDataClient>()
            .AddScoped<ToDoListManagerRestService>();

    internal static IServiceCollection InjectMessageBusSubscriber(this IServiceCollection services) =>
        services.AddHostedService<MessageBusSubscriber>();
}