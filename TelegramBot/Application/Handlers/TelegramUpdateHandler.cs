using TelegramBot.Contracts;
using TelegramBot.Infrastructure.Telegram;
using TelegramBot.Contracts;



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
                await telegramClient.SendTextMessageWithButtonAsync(
                    chatId,
                    "مرحباً بك في Todo Bot! اختر من القائمة:",
                    new TelegramButton(
                        "إضافة مهمة",
                        "add_todo"),
                    cancellationToken);
            
                logger.LogInformation(
                    "Start message sent to chat {ChatId}.",
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
