using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Contribution.EditContribution
{
    public class EditContributionCommandHandler : IRequestHandler<EditContributionCommand, Result<EditContributionDTO>>
    {
        private readonly IPartyRepository _partyRepository;

        public EditContributionCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<EditContributionDTO>> Handle(EditContributionCommand request, CancellationToken cancellationToken)
        {
            Core.Entities.Party partyInDatabase = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);
            var contri = partyInDatabase.Contributions.FirstOrDefault(c => c.Id == request.ID);
            if (contri == null)
            {
                return Result<EditContributionDTO>.Fail("Contribution not found.");
            }

            contri.Name = request.Name.FirstCharToUpper();
            contri.Tags = request.Tags.Select(tag => tag.FirstCharToUpper()).ToList();
            
            partyInDatabase.UpdateContributions();
            await _partyRepository.UpdatePartyAsync(partyInDatabase, cancellationToken);

            var contriDTO = new EditContributionDTO();

            return Result<EditContributionDTO>.Ok(contriDTO);
        }
    }
}
