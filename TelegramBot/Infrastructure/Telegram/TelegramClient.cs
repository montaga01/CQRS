using Telegram.Bot;

namespace TelegramBot.Infrastructure.Telegram;

public sealed class TelegramClient(
    ITelegramBotClient botClient) : ITelegramClient
{
    public async Task SendTextMessageAsync(
        long chatId,
        string text,
        CancellationToken cancellationToken = default)
    {
        await botClient.SendMessage(
            chatId,
            text,
            cancellationToken: cancellationToken);
    }
}
