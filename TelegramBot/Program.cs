using TelegramBot.Application;
using TelegramBot.Endpoints;
using TelegramBot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBotApplication();
builder.Services.AddBotInfrastructure(builder.Configuration);

var app = builder.Build();

app.MapTelegramWebhook();

app.Run();
