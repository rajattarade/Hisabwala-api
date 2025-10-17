using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Party.GetPartyInformation
{
    public class GetPartyInformationCommandHandler : IRequestHandler<GetPartyInformationCommand, Result<PartyInformationDTO>>
    {
        private readonly IPartyRepository _partyRepository;

        public GetPartyInformationCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<PartyInformationDTO>> Handle(GetPartyInformationCommand request, CancellationToken cancellationToken)
        {
            Core.Entities.Party party = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);

            var partyDTO = new PartyInformationDTO
            {
                PartyName = party.PartyName,
                PartyCode = party.PartyCode,
                CreatedDateTime = party.CreatedDateTime,
                Expenses = party.Expenses,
                Contributions = party.Contributions
            };

            return Result<PartyInformationDTO>.Ok(partyDTO);
        }
    }
}
