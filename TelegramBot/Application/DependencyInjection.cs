using Microsoft.Extensions.DependencyInjection;
using TelegramBot.Application.Handlers;
using TelegramBot.Application.State;


namespace TelegramBot.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddBotApplication(
        this IServiceCollection services)
    {
        services.AddScoped<TelegramUpdateHandler>();
        
        services.AddSingleton<IConversationStateStore,
            InMemoryConversationStateStore>();


        return services;
    }
}
