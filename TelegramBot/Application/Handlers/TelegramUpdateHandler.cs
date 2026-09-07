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
    
            return;
        }
    
        var chatId = update.Message.Chat.Id;
        var text = update.Message.Text?.Trim();
    
        if (string.IsNullOrWhiteSpace(text))
        {
            logger.LogInformation(
                "Message received from chat {ChatId} without text.",
                chatId);
    
            return;
        }
    
        switch (text.ToLowerInvariant())
        {
            case "/start":
                var statusMessage =
                    await telegramClient.SendTextMessageAsync(
                        chatId,
                        "جاري المعالجة...",
                        cancellationToken);
    
                await telegramClient.EditTextMessageAsync(
                    chatId,
                    statusMessage.MessageId,
                    "تمت المعالجة بنجاح!",
                    cancellationToken);
    
                logger.LogInformation(
                    "Start message updated for chat {ChatId}.",
                    chatId);
                break;
    
            default:
                logger.LogInformation(
                    "Unknown text received from chat {ChatId}: {Text}",
                    chatId,
                    text);
    
                await telegramClient.SendTextMessageAsync(
                    chatId,
                    "الأمر غير معروف. استخدم /start.",
                    cancellationToken);
                break;
        }
    }
}
