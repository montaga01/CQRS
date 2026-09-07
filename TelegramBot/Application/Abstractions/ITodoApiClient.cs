namespace TelegramBot.Application.Abstractions;

public interface ITodoApiClient
{
    Task CreateTodoAsync(
        string title,
        CancellationToken cancellationToken = default);
}
