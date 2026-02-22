using MediatR;
using TwitterClone.Application.Interfaces;
using TwitterClone.Domain.Common;

namespace TwitterClone.Application.Tweets.Queries;

public record CommentDto(Guid Id, string Content, Guid UserId, string Username, DateTime CreatedAt);
public record TweetDto(Guid Id, string Content, Guid UserId, string Username, DateTime CreatedAt, IEnumerable<CommentDto> Comments);

public record GetFeedQuery() : IRequest<Result<IEnumerable<TweetDto>>>;

public class GetFeedQueryHandler : IRequestHandler<GetFeedQuery, Result<IEnumerable<TweetDto>>>
{
    private readonly ITweetRepository _repository;

    public GetFeedQueryHandler(ITweetRepository repository) => _repository = repository;

    public async Task<Result<IEnumerable<TweetDto>>> Handle(GetFeedQuery request, CancellationToken cancellationToken)
    {
        var tweets = await _repository.GetFeedAsync(cancellationToken);
        var dtos = tweets.Select(t => new TweetDto(
            t.Id,
            t.Content,
            t.UserId,
            t.User?.Username ?? "Unknown",
            t.CreatedAt,
            t.Comments
                .OrderBy(c => c.CreatedAt)
                .Select(c => new CommentDto(c.Id, c.Content, c.UserId, c.User?.Username ?? "Unknown", c.CreatedAt))
        ));
        return Result<IEnumerable<TweetDto>>.Success(dtos);
    }
}