using TelegramBot.Contracts;
using TelegramBot.Infrastructure.Telegram;


namespace TelegramBot.Application.Handlers;

public sealed class TelegramUpdateHandler(
    ILogger<TelegramUpdateHandler> logger,
    ITelegramClient telegramClient)
{
    public async Task HandleAsync(
        TelegramUpdate update,
        CancellationToken cancellationToken)
    {
        if (update.Message is null)
        {
            logger.LogInformation(
                "Update {UpdateId} has no message.",
                update.UpdateId);

            return Task.CompletedTask;
        }

        var chatId = update.Message.Chat.Id;
        var text = update.Message.Text?.Trim();

        if (string.IsNullOrWhiteSpace(text))
        {
            logger.LogInformation(
                "Message received from chat {ChatId} without text.",
                chatId);

            return Task.CompletedTask;
        }

        switch (text.ToLowerInvariant())
        {
            case "/start":
                await telegramClient.SendTextMessageAsync(
                    chatId,
                    "مرحباً بك في Todo Bot!",
                    cancellationToken);
            
                logger.LogInformation(
                    "Start reply sent to chat {ChatId}.",
                    chatId);
                break;


            default:
                logger.LogInformation(
                    "Unknown text received from chat {ChatId}: {Text}",
                    chatId,
                    text);
                break;
        }

        return Task.CompletedTask;
    }
}
