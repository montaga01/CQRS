using System.Net.Http.Json;
using TelegramBot.Application.Abstractions;

namespace TelegramBot.Infrastructure.BackendApi;

public sealed class TodoApiClient(
    HttpClient httpClient ) : ITodoApiClient
{
    public async Task CreateTodoAsync(
        string title,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            "api/todos",
            new
            {
                title
            },
            cancellationToken );

        response.EnsureSuccessStatusCode();
    }
}
