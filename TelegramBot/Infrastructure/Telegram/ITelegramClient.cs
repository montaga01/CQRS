using TelegramBot.Contracts;

namespace TelegramBot.Infrastructure.Telegram;

public sealed record TelegramSentMessage(
    long ChatId,
    int MessageId);

public interface ITelegramClient
{
    Task<TelegramSentMessage> SendTextMessageAsync(
        long chatId,
        string text,
        CancellationToken cancellationToken = default);

    Task<TelegramSentMessage> SendTextMessageWithButtonAsync(
        long chatId,
        string text,
        TelegramButton button,
        CancellationToken cancellationToken = default);

    Task EditTextMessageAsync(
        long chatId,
        int messageId,
        string text,
        CancellationToken cancellationToken = default);

    Task AnswerCallbackQueryAsync(
        string callbackQueryId,
        string? text = null,
        CancellationToken cancellationToken = default);

}
