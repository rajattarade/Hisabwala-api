using Hisabwala.Application.Features.Party;
using Hisabwala.Application.Features.Party.GeneratePartyCode;
using Hisabwala.Core.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
public class PartyController : ControllerBase
{
    private readonly IMediator _mediator;

    public PartyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<Result<PartyCodeDTO>> GeneratePartyCode([FromBody] GeneratePartyCodeCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }
}
