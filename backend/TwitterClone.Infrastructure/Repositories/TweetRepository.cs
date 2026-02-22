using Microsoft.EntityFrameworkCore;
using TwitterClone.Application.Interfaces;
using TwitterClone.Domain.Entities;
using TwitterClone.Infrastructure.Data;

namespace TwitterClone.Infrastructure.Repositories;

public class TweetRepository : ITweetRepository
{
    private readonly TwitterDbContext _context;

    public TweetRepository(TwitterDbContext context) => _context = context;

    public async Task AddAsync(Tweet tweet, CancellationToken cancellationToken)
    {
        await _context.Tweets.AddAsync(tweet, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Tweet>> GetFeedAsync(CancellationToken cancellationToken)
    {
        return await _context.Tweets
        .Include(t => t.User)
        .Include(t => t.Comments.OrderBy(c => c.CreatedAt))
            .ThenInclude(c => c.User)
        .OrderByDescending(t => t.CreatedAt)
        .Take(50)
        .AsNoTracking()
        .ToListAsync(cancellationToken);
    }

    public Task<Tweet?> GetByIdAsync(Guid id, CancellationToken ct) => _context.Tweets.FirstOrDefaultAsync(t => t.Id == id, ct);
    public Task<IEnumerable<Tweet>> GetByUserIdAsync(Guid userId, CancellationToken ct) => throw new NotImplementedException(); // @TODO: not implemented yet
    public Task UpdateAsync(Tweet tweet, CancellationToken ct) => _context.SaveChangesAsync(ct);
    public Task DeleteAsync(Tweet tweet, CancellationToken ct) { _context.Tweets.Remove(tweet); return _context.SaveChangesAsync(ct); }

}