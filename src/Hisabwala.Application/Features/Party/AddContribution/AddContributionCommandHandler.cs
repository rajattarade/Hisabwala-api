using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Features.Party.AddExpense;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Common;
using Hisabwala.Core.Entities;
using MediatR;
using MongoDB.Bson;

namespace Hisabwala.Application.Features.Party.AddContribution
{
    public class AddContributionCommandHandler : IRequestHandler<AddContributionCommand, Result<AddContributionDTO>>
    {
        private readonly IPartyRepository _partyRepository;

        public AddContributionCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<AddContributionDTO>> Handle(AddContributionCommand request, CancellationToken cancellationToken)
        {
            Core.Entities.Party party = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);

            var contri = new Core.Entities.Contribution
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Tags = request.Tags
            };

            party.AddContribution(contri);

            await _partyRepository.UpdatePartyAsync(party, cancellationToken);

            AddContributionDTO contriDTO = new AddContributionDTO
            {
                Id = contri.Id
            };

            return Result<AddContributionDTO>.Ok(contriDTO);
        }
    }
}
