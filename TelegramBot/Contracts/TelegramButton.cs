namespace TelegramBot.Contracts;

public sealed record TelegramButton(
    string Text,
    string CallbackData);
