namespace TelegramBot.Infrastructure.Telegram;

public interface ITelegramClient
{
    Task SendTextMessageAsync(
        long chatId,
        string text,
        CancellationToken cancellationToken = default);
}
