using TelegramBot.Application.Abstractions;
using TelegramBot.Application.State;
using TelegramBot.Contracts;
using TelegramBot.Infrastructure.Telegram;

namespace TelegramBot.Application.Handlers;

public sealed class TelegramUpdateHandler(
    ILogger<TelegramUpdateHandler> logger,
    ITelegramClient telegramClient,
    ITodoApiClient todoApiClient,
    IConversationStateStore stateStore)
{
    public async Task HandleAsync(
        TelegramUpdate update,
        CancellationToken cancellationToken)
    {
        // أولاً: معالجة ضغط الأزرار
        if (update.CallbackQuery is not null)
        {
            var callbackData = update.CallbackQuery.Data;
            var callbackChatId =
                update.CallbackQuery.Message?.Chat.Id;

            await telegramClient.AnswerCallbackQueryAsync(
                update.CallbackQuery.Id,
                cancellationToken: cancellationToken);

            if (callbackChatId is null)
            {
                logger.LogWarning(
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
                stateStore.WaitForTodoTitle(
                    callbackChatId.Value);

                await telegramClient.SendTextMessageAsync(
                    callbackChatId.Value,
                    "اكتب اسم المهمة:",
                    cancellationToken);
            }

            return;
        }

        // ثانياً: التأكد من وجود رسالة نصية
        if (update.Message is null)
        {
            logger.LogInformation(
                "Update {UpdateId} has no message.",
                update.UpdateId);

            return;
        }

        var messageChatId = update.Message.Chat.Id;
        var text = update.Message.Text?.Trim();

        if (string.IsNullOrWhiteSpace(text))
        {
            logger.LogInformation(
                "Message received from chat {ChatId} without text.",
                messageChatId);

            return;
        }

        // ثالثاً: إذا كان المستخدم ينتظر إدخال اسم المهمة
        if (stateStore.IsWaitingForTodoTitle(messageChatId))
        {
            var statusMessage =
                await telegramClient.SendTextMessageAsync(
                    messageChatId,
                    "جاري إضافة المهمة...",
                    cancellationToken);

            try
            {
                await todoApiClient.CreateTodoAsync(
                    text,
                    cancellationToken);

                stateStore.Clear(messageChatId);

                await telegramClient.EditTextMessageAsync(
                    messageChatId,
                    statusMessage.MessageId,
                    $"تمت إضافة المهمة بنجاح:\n{text}",
                    cancellationToken);
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "Failed to create todo for chat {ChatId}.",
                    messageChatId);

                await telegramClient.EditTextMessageAsync(
                    messageChatId,
                    statusMessage.MessageId,
                    "حدث خطأ أثناء إضافة المهمة. حاول مرة أخرى.",
                    cancellationToken);
            }

            return;
        }

        // رابعاً: معالجة الأوامر العادية
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
                await telegramClient.SendTextMessageAsync(
                    messageChatId,
                    "الأمر غير معروف. استخدم /start.",
                    cancellationToken);
                break;
        }
    }
}
