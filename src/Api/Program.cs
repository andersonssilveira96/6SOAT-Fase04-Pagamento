using Api.Extensions;
using Api.Helper;
using Application;
using Domain.Consumer;
using Infra.Data;
using Infra.Gateway;
using Infra.MessageBroker;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TechChallenge Pagamento API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
builder.Services.Configure<AuthenticationCognitoOptions>(builder.Configuration.GetSection("CognitoConfig"));

builder.Services.AddApplicationService();
builder.Services.AddInfraDataServices();
builder.Services.AddInfraGatewayServices();
builder.Services.AddInfraMessageBrokerServices();

builder.Services.AddAuthenticationConfig();

var app = builder.Build();

// Invocar o serviço
using var scope = app.Services.CreateScope();
var messageConsumer = scope.ServiceProvider.GetRequiredService<IMessageBrokerConsumer>();
_ = Task.Run(() => messageConsumer.ReceiveMessageAsync());

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
