namespace TwitterClone.Domain.Entities;

public class Comment
{
    public Guid Id { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public Guid TweetId { get; private set; }
    public Guid UserId { get; private set; }

    public Tweet Tweet { get; private set; } = null!;
    public User User { get; private set; } = null!;

    protected Comment() { }

    public Comment(Guid tweetId, Guid userId, string content)
    {
        Id = Guid.NewGuid();
        TweetId = tweetId;
        UserId = userId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }
}