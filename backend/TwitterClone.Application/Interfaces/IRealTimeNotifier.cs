namespace TwitterClone.Application.Interfaces;

public interface IRealTimeNotifier
{
    Task BroadcastNewTweetAsync(object tweetDto, CancellationToken cancellationToken);
}
