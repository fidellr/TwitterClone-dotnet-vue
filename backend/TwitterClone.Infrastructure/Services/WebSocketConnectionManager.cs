using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace TwitterClone.Infrastructure.Services;

public class WebSocketConnectionManager
{
    private readonly ConcurrentDictionary<string, WebSocket> _sockets = new();
    
    public void AddSocket(string id, WebSocket socket) => _sockets.TryAdd(id, socket);

    public async Task RemoveSocketAsync(string id, CancellationToken cancellationToken)
    {
        if (_sockets.TryRemove(id, out var socket) && socket.State != WebSocketState.Closed)
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", cancellationToken);
        }
    }

    public IEnumerable<WebSocket> GetAll() => _sockets.Values.Where(s => s.State == WebSocketState.Open);
}