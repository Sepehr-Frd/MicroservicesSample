using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RedditMockup.Business.Contracts;
using RedditMockup.Business.DomainEntityBusinesses;
using RedditMockup.Business.PublicBusinesses;
using RedditMockup.Common.Constants;
using RedditMockup.Common.Dtos;
using RedditMockup.Common.Profiles;
using RedditMockup.Common.Validations;
using RedditMockup.DataAccess;
using RedditMockup.DataAccess.Context;
using RedditMockup.DataAccess.Contracts;
using RedditMockup.ExternalService.RabbitMQService;
using RedditMockup.ExternalService.RabbitMQService.Contracts;
using RedditMockup.Model.Entities;
using Serilog;
using Sieve.Services;
using System.Text.Json.Serialization;

namespace RedditMockup.Web;

internal static class DependencyInjectionExtension
{
    internal static IServiceCollection InjectApi(this IServiceCollection services) =>
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })
            .Services;

    internal static IServiceCollection InjectCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

    internal static IServiceCollection InjectSwagger(this IServiceCollection services) =>
        services.AddSwaggerGen();

    internal static IServiceCollection InjectUnitOfWork(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>();

    internal static IServiceCollection InjectContext(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        if (environment.IsEnvironment("Testing"))
        {
            return services.AddDbContext<RedditMockupContext>(options => options.UseInMemoryDatabase("RedditMockup"));
        }

        return services.AddDbContext<RedditMockupContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
            options.EnableSensitiveDataLogging();
        });
    }

    internal static IServiceCollection InjectSerilog(this IServiceCollection services, IConfiguration configuration) =>
        services.AddSerilog(x => x.ReadFrom.Configuration(configuration));

    internal static IServiceCollection InjectSieve(this IServiceCollection services) =>
        services.AddScoped<ISieveProcessor, SieveProcessor>();

    internal static IServiceCollection InjectAuthentication(this IServiceCollection services) =>
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
            .AddCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            })
            .Services
            .AddAuthorization(options =>
            {
                options.AddPolicy(PolicyConstants.Admin,
                    policy => policy.RequireClaim(RoleConstants.Admin));
                options.AddPolicy(PolicyConstants.User,
                    policy => policy.RequireClaim(RoleConstants.User));
            });

    internal static IServiceCollection InjectBusinesses(this IServiceCollection services) =>
        services.AddScoped<IBaseBusiness<User, UserDto>, UserBusiness>()
            .AddScoped<IBaseBusiness<Answer, AnswerDto>, AnswerBusiness>()
            .AddScoped<IBaseBusiness<Question, QuestionDto>, QuestionBusiness>()
            .AddScoped<IPublicBaseBusiness<Answer, AnswerDto>, PublicAnswerBusiness>()
            .AddScoped<IPublicBaseBusiness<Question, QuestionDto>, PublicQuestionBusiness>()
            .AddScoped<AccountBusiness>();

    internal static IServiceCollection InjectFluentValidation(this IServiceCollection services) =>
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<RoleValidator>();

    internal static IServiceCollection InjectAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(typeof(AnswerProfile).Assembly);

    internal static IServiceCollection InjectRabbitMq(this IServiceCollection services) =>
        services.AddSingleton<IMessageBusClient, MessageBusClient>();

    internal static IServiceCollection InjectGrpc(this IServiceCollection services) =>
        services.AddGrpc(configure =>
        {
            configure.EnableDetailedErrors = true;
        }).Services;
}