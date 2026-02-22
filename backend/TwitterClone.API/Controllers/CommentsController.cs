using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Application.Comments.Commands;

namespace TwitterClone.API.Controllers;

[Authorize] // requires JWT Token
[ApiController]
[Route("api/tweets/{tweetId}/comments")]
public class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public CommentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> AddComment(Guid tweetId, [FromBody] AddCommentCommand command, CancellationToken ct)
    {
        // enforce the tweetId from the route matches body
        var result = await _mediator.Send(command with { TweetId = tweetId }, ct);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(new { Id = result.Value });
    }
}