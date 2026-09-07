using TelegramBot.Application.Handlers;
using TelegramBot.Endpoints;
using TelegramBot.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<TelegramUpdateHandler>();

builder.Services.Configure<TelegramOptions>(
    builder.Configuration.GetSection("Telegram"));

var app = builder.Build();

app.MapTelegramWebhook();

app.Run();
