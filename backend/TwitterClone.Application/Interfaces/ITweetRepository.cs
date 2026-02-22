using TwitterClone.Domain.Entities;
namespace TwitterClone.Application.Interfaces;

public interface ITweetRepository
{
    Task<Tweet?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Tweet>> GetFeedAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Tweet>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task AddAsync(Tweet tweet, CancellationToken cancellationToken);
    Task UpdateAsync(Tweet tweet, CancellationToken cancellationToken);
    Task DeleteAsync(Tweet tweet, CancellationToken cancellationToken);
}