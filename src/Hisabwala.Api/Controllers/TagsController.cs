using Hisabwala.Application.Features.Tags.GetTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
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
}
