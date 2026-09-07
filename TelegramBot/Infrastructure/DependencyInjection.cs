using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using TelegramBot.Application.Abstractions;
using TelegramBot.Configuration;
using TelegramBot.Infrastructure.Telegram;
using TelegramBot.Infrastructure.BackendApi;

namespace TelegramBot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBotInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<TelegramOptions>(
            configuration.GetSection("Telegram"));

        services.AddSingleton<ITelegramBotClient>(serviceProvider =>
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

        services.AddScoped<ITelegramClient, TelegramClient>();

        services.AddHttpClient<ITodoApiClient, TodoApiClient>(client =>
        {   
            var baseUrl = configuration["Api:BaseUrl"];   
           
            if (string.IsNullOrWhiteSpace(baseUrl))   
            {   
                throw new InvalidOperationException(   
                    "API BaseUrl is missing.");   
            }   
           
            client.BaseAddress = new Uri(baseUrl);   
        });   

        return services;
    }
}
