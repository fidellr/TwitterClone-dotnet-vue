using TwitterClone.Application.Interfaces;
using TwitterClone.Domain.Entities;
using TwitterClone.Infrastructure.Data;

namespace TwitterClone.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly TwitterDbContext _context;
    public CommentRepository(TwitterDbContext context) => _context = context;

    public async Task AddAsync(Comment comment, CancellationToken ct)
    {
        await _context.Comments.AddAsync(comment, ct);
        await _context.SaveChangesAsync(ct);
    }
}