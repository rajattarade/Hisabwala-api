using Hisabwala.Application.Features.Party.AddContribution;
using Hisabwala.Application.Features.Party.AddExpense;
using Hisabwala.Application.Features.Party.DeleteContribution;
using Hisabwala.Application.Features.Party.DeleteExpense;
using Hisabwala.Application.Features.Party.EditContribution;
using Hisabwala.Application.Features.Party.EditExpense;
using Hisabwala.Application.Features.Party.GeneratePartyCode;
using Hisabwala.Application.Features.Party.GetPartyInformation;
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

    [HttpPost]
    public async Task<Result<PartyInformationDTO>> GetPartyInformation([FromBody] GetPartyInformationCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpPost]
    public async Task<Result<AddExpenseDTO>> AddExpense([FromBody] AddExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpPost]
    public async Task<Result<EditExpenseDTO>> EditExpense([FromBody] EditExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpPost]
    public async Task<Result<DeleteExpenseDTO>> DeleteExpense([FromBody] DeleteExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpPost]
    public async Task<Result<AddContributionDTO>> AddContribution([FromBody] AddContributionCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpPost]
    public async Task<Result<EditContributionDTO>> EditContribution([FromBody] EditContributionCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }

    [HttpPost]
    public async Task<Result<DeleteContributionDTO>> DeleteContribution([FromBody] DeleteContributionCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }
}
