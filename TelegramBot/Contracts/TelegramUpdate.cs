using System.Text.Json.Serialization;

namespace TelegramBot.Contracts;

public sealed class TelegramUpdate
{
    [JsonPropertyName("update_id")]
    public long UpdateId { get; set; }

    [JsonPropertyName("message")]
    public TelegramMessage? Message { get; set; }
}

public sealed class TelegramMessage
{
    [JsonPropertyName("chat")]
    public TelegramChat Chat { get; set; } = new();

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}

public sealed class TelegramChat
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}
