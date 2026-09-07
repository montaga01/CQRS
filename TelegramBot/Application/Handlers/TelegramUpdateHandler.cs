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
        // معالجة ضغط Inline Button
        if (update.CallbackQuery is not null)
        {
            var callbackData = update.CallbackQuery.Data;
            var callbackChatId = update.CallbackQuery.Message?.Chat.Id;

            if (callbackChatId is null)
            {
                logger.LogInformation(
                    "Callback query has no chat information.");

                return;
            }

            logger.LogInformation(
                "Button {Data} clicked in chat {ChatId}.",
                callbackData,
                callbackChatId.Value);

            if (string.Equals(
                    callbackData,
                    "add_todo",
                    StringComparison.OrdinalIgnoreCase))
            {
                await telegramClient.SendTextMessageAsync(
                    callbackChatId.Value,
                    "اكتب اسم المهمة:",
                    cancellationToken);
            }

            return;
        }

        // التأكد من وجود Message
        if (update.Message is null)
        {
            logger.LogInformation(
                "Update {UpdateId} has no message.",
                update.UpdateId);

            return;
        }

        // استخراج بيانات الرسالة
        var messageChatId = update.Message.Chat.Id;
        var text = update.Message.Text?.Trim();

        // التأكد من وجود نص
        if (string.IsNullOrWhiteSpace(text))
        {
            logger.LogInformation(
                "Message received from chat {ChatId} without text.",
                messageChatId);

            return;
        }

        // معالجة الأوامر النصية
        switch (text.ToLowerInvariant())
        {
            case "/start":
                await telegramClient.SendTextMessageWithButtonAsync(
                    messageChatId,
                    "مرحباً بك في Todo Bot! اختر من القائمة:",
                    new TelegramButton(
                        "إضافة مهمة",
                        "add_todo"),
                    cancellationToken);

                logger.LogInformation(
                    "Start message sent to chat {ChatId}.",
                    messageChatId);
                break;

            default:
                logger.LogInformation(
                    "Unknown text received from chat {ChatId}: {Text}",
                    messageChatId,
                    text);

                await telegramClient.SendTextMessageAsync(
                    messageChatId,
                    "الأمر غير معروف. استخدم /start.",
                    cancellationToken);
                break;
        }
    }
}
