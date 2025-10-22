using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Contribution.DeleteContribution
{
    public class DeleteContributionCommandHandler : IRequestHandler<DeleteContributionCommand, Result<DeleteContributionDTO>>
    {
        private readonly IPartyRepository _partyRepository;

        public DeleteContributionCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<DeleteContributionDTO>> Handle(DeleteContributionCommand request, CancellationToken cancellationToken)
        {
            Core.Entities.Party partyInDatabase = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);
            var contri = partyInDatabase.Contributions.FirstOrDefault(c => c.Id == request.ID);
            if (contri == null)
            {
                return Result<DeleteContributionDTO>.Fail("Contribution not found.");
            }

            partyInDatabase.Contributions.Remove(contri);
            partyInDatabase.UpdateContributions();
            await _partyRepository.UpdatePartyAsync(partyInDatabase, cancellationToken);

            var contriDTO = new DeleteContributionDTO();

            return Result<DeleteContributionDTO>.Ok(contriDTO);
        }
    }
}
