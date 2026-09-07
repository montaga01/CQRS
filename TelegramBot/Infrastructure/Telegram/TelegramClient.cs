using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Contracts;

namespace TelegramBot.Infrastructure.Telegram;

public sealed class TelegramClient(
    ITelegramBotClient botClient) : ITelegramClient
{
    public async Task<TelegramSentMessage> SendTextMessageAsync(
        long chatId,
        string text,
        CancellationToken cancellationToken = default)
    {
        var message = await botClient.SendMessage(
            chatId,
            text,
            cancellationToken: cancellationToken);

        return new TelegramSentMessage(
            message.Chat.Id,
            message.Id);
    }

    public async Task<TelegramSentMessage> SendTextMessageWithButtonAsync(
        long chatId,
        string text,
        TelegramButton button,
        CancellationToken cancellationToken = default)
    {
        var keyboard = new InlineKeyboardMarkup(
            InlineKeyboardButton.WithCallbackData(
                button.Text,
                button.CallbackData));

        var message = await botClient.SendMessage(
            chatId,
            text,
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);

        return new TelegramSentMessage(
            message.Chat.Id,
            message.Id);
    }

    public async Task EditTextMessageAsync(
        long chatId,
        int messageId,
        string text,
        CancellationToken cancellationToken = default)
    {
        await botClient.EditMessageText(
            chatId,
            messageId,
            text,
            cancellationToken: cancellationToken);
    }
}
