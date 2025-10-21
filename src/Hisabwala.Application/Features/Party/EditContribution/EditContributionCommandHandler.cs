using Hisabwala.Application.Features.Party.AddContribution;
using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;
using MediatR;
using MongoDB.Bson;

namespace Hisabwala.Application.Features.Party.EditContribution
{
    public class EditContributionCommandHandler : IRequestHandler<EditContributionCommand, Result<EditContributionDTO>>
    {
        private readonly IPartyRepository _partyRepository;
        private Core.Entities.Party partyInDatabase;

        public EditContributionCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<EditContributionDTO>> Handle(EditContributionCommand request, CancellationToken cancellationToken)
        {
            partyInDatabase = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);
            var contri = partyInDatabase.Contributions.First(c => c.Id == request.ID);

            contri.Name = request.Name.FirstCharToUpper();
            contri.Tags = request.Tags.Select(tag => tag.FirstCharToUpper()).ToList();
            
            partyInDatabase.UpdateContributions();
            await _partyRepository.UpdatePartyAsync(partyInDatabase, cancellationToken);

            var contriDTO = new EditContributionDTO();

            return Result<EditContributionDTO>.Ok(contriDTO);
        }
    }
}
