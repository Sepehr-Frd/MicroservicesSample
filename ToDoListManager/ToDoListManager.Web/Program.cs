using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Settings.Configuration;
using ToDoListManager.Common.Helpers;
using ToDoListManager.DataAccess.Context;
using ToDoListManager.Services.Grpc;
using ToDoListManager.Web;
using ToDoListManager.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// macOS Configuration for gRPC over HTTP 2.0 Without TLS --------------------------->

/*
 builder.WebHost.ConfigureKestrel(options =>
{
    // Setup a HTTP/2 endpoint without TLS.

    options.ListenLocalhost(6000, o => o.Protocols =
        HttpProtocols.Http2);
});
*/

// <---------------------------------------------------------------------------------

builder.Configuration.AddEnvironmentVariables();

builder.Logging.ClearProviders();

Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration, new ConfigurationReaderOptions
    {
        SectionName = "InternalSerilog"
    })
    .CreateBootstrapLogger();

Log.Information("Application setup started.");

try
{
    builder.Services
        .AddEndpointsApiExplorer()
        .InjectApi()
        .InjectCors()
        .InjectSwagger()
        .AddHttpContextAccessor()
        .InjectUnitOfWork()
        .InjectSerilog(builder.Configuration)
        .InjectAuthentication()
        .InjectContext(builder.Configuration, builder.Environment)
        .InjectBusinesses()
        .InjectFluentValidation()
        .InjectRabbitMq()
        .InjectGrpc()
        .AddHealthChecks();

    MapperHelper.RegisterMapperConfigurations();

    var app = builder.Build();

    await using var scope = app.Services.CreateAsyncScope();

    await using var context = scope.ServiceProvider.GetRequiredService<ToDoListManagerDbContext>();

    app.UseSwagger()
        .UseSwaggerUI();

    if (app.Environment.IsEnvironment("Testing"))
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    else
    {
        await context.Database.MigrateAsync();
        //app.UseHsts();
    }

    app
        .UseMiddleware<CustomExceptionMiddleware>()
        //.UseHttpsRedirection()
        .UseCors("AllowAnyOrigin")
        .UseRouting()
        .UseAuthentication()
        .UseAuthorization()
        .UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/healthcheck");
            endpoints.MapGrpcService<GrpcService>();
            endpoints.MapGet("/protos/ToDoListManager.proto", async httpContext =>
            {
                await httpContext
                    .Response
                    .WriteAsync(File.ReadAllText("../ToDoListManager.Model/Protos/ToDoListManager.proto"));
            });
        });
    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Error(exception, "Program stopped due to a {ExceptionType} exception", exception.GetType());
    throw;
}
finally
{
    Log.CloseAndFlush();
}