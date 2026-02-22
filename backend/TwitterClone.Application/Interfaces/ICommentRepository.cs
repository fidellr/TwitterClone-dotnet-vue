using TwitterClone.Domain.Entities;

namespace TwitterClone.Application.Interfaces;

public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken cancellationToken);
}