namespace TwitterClone.Domain.Entities;

public class Tweet
{
    public Guid Id { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid UserId { get; private set; }

    public User User { get; private set; } = null!;
    // Comment
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

    protected Tweet() { }

    public Tweet(Guid userId, string content)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Content = content;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateContent(string newContent)
    {
        Content = newContent;
        UpdatedAt = DateTime.UtcNow;
    }
}