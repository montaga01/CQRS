using Microsoft.AspNetCore.Mvc;
using TelegramBot.Application.Handlers;
using TelegramBot.Contracts;

namespace TelegramBot.Endpoints;

public static class TelegramWebhookEndpoint
{
    public static IEndpointRouteBuilder MapTelegramWebhook(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/telegram/webhook",
            async (
                TelegramUpdate update,
                [FromServices] TelegramUpdateHandler handler,
                CancellationToken cancellationToken) =>
            {
                await handler.HandleAsync(
                    update,
                    cancellationToken);

                return Results.Ok();
            });

        return endpoints;
    }
}
