using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Party.GeneratePartyCode
{
    public class GeneratePartyCodeCommand : IRequest<Result<PartyCodeDTO>>
    {
        public string PartyName { get; set; } = string.Empty;
    }
}
