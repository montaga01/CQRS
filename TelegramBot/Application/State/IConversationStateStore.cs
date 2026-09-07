using System.Collections.Concurrent;

namespace TelegramBot.Application.State;

public interface IConversationStateStore
{
    void WaitForTodoTitle(long chatId);

    bool IsWaitingForTodoTitle(long chatId);

    void Clear(long chatId);
}

public sealed class InMemoryConversationStateStore
    : IConversationStateStore
{
    private readonly ConcurrentDictionary<long, bool> _waitingChats = new();

    public void WaitForTodoTitle(long chatId)
    {
        _waitingChats[chatId] = true;
    }

    public bool IsWaitingForTodoTitle(long chatId)
    {
        return _waitingChats.ContainsKey(chatId);
    }

    public void Clear(long chatId)
    {
        _waitingChats.TryRemove(chatId, out _);
    }
}
