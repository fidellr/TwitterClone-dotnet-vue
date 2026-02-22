using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using TwitterClone.Application.Interfaces;

namespace TwitterClone.Infrastructure.Services;

public class RealTimeNotifier : IRealTimeNotifier
{
    private readonly WebSocketConnectionManager _manager;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public RealTimeNotifier(WebSocketConnectionManager manager) => _manager = manager;

    public async Task BroadcastNewTweetAsync(object tweetDto, CancellationToken cancellationToken)
    {
        var message = JsonSerializer.Serialize(new { Type = "NEW_TWEET", Payload = tweetDto }, _jsonOptions);
        var bytes = Encoding.UTF8.GetBytes(message);
        var arraySegment = new ArraySegment<byte>(bytes);

        var tasks = _manager.GetAll().Select(socket =>
        socket.SendAsync(arraySegment, WebSocketMessageType.Text, true, cancellationToken));

        await Task.WhenAll(tasks);
    }
}