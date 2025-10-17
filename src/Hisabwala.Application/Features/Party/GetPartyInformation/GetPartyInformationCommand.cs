using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Party.GetPartyInformation
{
    public class GetPartyInformationCommand : IRequest<Result<PartyInformationDTO>>
    {
        public string PartyCode { get; set; } = null!;
    }
}
