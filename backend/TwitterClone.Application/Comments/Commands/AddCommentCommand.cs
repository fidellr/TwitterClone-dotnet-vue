using MediatR;
using TwitterClone.Application.Interfaces;
using TwitterClone.Domain.Common;
using TwitterClone.Domain.Entities;

namespace TwitterClone.Application.Comments.Commands;

public record AddCommentCommand(Guid TweetId, Guid UserId, string Content) : IRequest<Result<Guid>>;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result<Guid>>
{
    private readonly ICommentRepository _repository;

    public AddCommentCommandHandler(ICommentRepository repository) => _repository = repository;
    
    public async Task<Result<Guid>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Comment(request.TweetId, request.UserId, request.Content);
        await _repository.AddAsync(comment, cancellationToken);
        return Result<Guid>.Success(comment.Id);
    }
}
