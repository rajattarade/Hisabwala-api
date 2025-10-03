using Hisabwala.Application.Features.Tags;
using Hisabwala.Application.Features.Tags.AddTag;
using Hisabwala.Core.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
public class TagsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TagsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<TagDto>>> GetTags()
    {
        var result = await _mediator.Send(new GetTagsQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<Result<TagDto>> AddTag([FromBody] AddTagCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }
}
