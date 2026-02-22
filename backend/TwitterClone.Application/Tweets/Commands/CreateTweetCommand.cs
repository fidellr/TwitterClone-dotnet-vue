using MediatR;
using FluentValidation;
using TwitterClone.Domain.Entities;
using TwitterClone.Domain.Common;
using TwitterClone.Application.Interfaces;
using TwitterClone.Application.Tweets.Queries;

namespace TwitterClone.Application.Tweets.Commands;

public record CreateTweetCommand(Guid UserId, string Content) : IRequest<Result<Guid>>;

public class CreateTweetValidator : AbstractValidator<CreateTweetCommand>
{
    public CreateTweetValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(280);
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class CreateTweetCommandHandler : IRequestHandler<CreateTweetCommand, Result<Guid>>
{
    private readonly ITweetRepository _tweetRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRealTimeNotifier _notifier;

    public CreateTweetCommandHandler(
        ITweetRepository tweetRepository,
        IUserRepository userRepository,
        IRealTimeNotifier notifier)
    {
        _tweetRepository = tweetRepository;
        _userRepository = userRepository;
        _notifier = notifier;
    }

    public async Task<Result<Guid>> Handle(CreateTweetCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null) return Result<Guid>.Failure("User not found.");

        var tweet = new Tweet(request.UserId, request.Content);
        await _tweetRepository.AddAsync(tweet, cancellationToken);

        var tweetDto = new TweetDto(
            tweet.Id,
            tweet.Content,
            tweet.UserId,
            user.Username,
            tweet.CreatedAt,
            new List<CommentDto>()
        );

        // broadcast real-time feed update
        await _notifier.BroadcastNewTweetAsync(tweetDto, cancellationToken);

        return Result<Guid>.Success(tweet.Id);
    }
}