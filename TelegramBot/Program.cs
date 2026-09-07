using TelegramBot.Application.Handlers;
using TelegramBot.Endpoints;
using TelegramBot.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.Infrastructure.Telegram;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<TelegramUpdateHandler>();

builder.Services.Configure<TelegramOptions>(
    builder.Configuration.GetSection("Telegram"));

builder.Services.AddSingleton<ITelegramBotClient>(serviceProvider =>
{
    var options = serviceProvider
        .GetRequiredService<IOptions<TelegramOptions>>()
        .Value;

    if (string.IsNullOrWhiteSpace(options.BotToken))
    {
        throw new InvalidOperationException(
            "Telegram bot token is missing.");
    }

    return new TelegramBotClient(options.BotToken);
});

builder.Services.AddScoped<ITelegramClient, TelegramClient>();


var app = builder.Build();

app.MapTelegramWebhook();

app.Run();
