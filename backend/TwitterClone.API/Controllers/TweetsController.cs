using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Application.Tweets.Commands;
using TwitterClone.Application.Tweets.Queries;

namespace TwitterClone.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TweetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TweetsController(IMediator mediator) => _mediator = mediator;

    [HttpGet("feed")]
    public async Task<IActionResult> GetFeed(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFeedQuery(), ct);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTweetCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetFeed), new { id = result.Value }, result.Value);
    }
}