using Microsoft.AspNetCore.SignalR;

namespace ServerAPI.Hubs
{
    public class TelemetryHub : Hub
    {
        public Task SubscribeToClient(string clientId) => Groups.AddToGroupAsync(Context.ConnectionId, $"client:{clientId}");
        public Task UnsubscribeFromClient(string clientId) => Groups.RemoveFromGroupAsync(Context.ConnectionId, $"client:{clientId}");
    }
}
