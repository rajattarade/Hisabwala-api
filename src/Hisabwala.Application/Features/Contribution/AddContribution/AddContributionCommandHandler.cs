using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;
using Hisabwala.Core.Entities;
using MediatR;
using MongoDB.Bson;

namespace Hisabwala.Application.Features.Contribution.AddContribution
{
    public class AddContributionCommandHandler : IRequestHandler<AddContributionCommand, Result<AddContributionDTO>>
    {
        private readonly IPartyRepository _partyRepository;
        private Core.Entities.Party partyInDatabase;
        public AddContributionCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<AddContributionDTO>> Handle(AddContributionCommand request, CancellationToken cancellationToken)
        {
            partyInDatabase = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);

            var contri = new Core.Entities.Contribution
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name.FirstCharToUpper(),
                Tags = request.Tags.Select(tag => tag.FirstCharToUpper()).ToList()
            };

            AddContribution(contri);

            await _partyRepository.UpdatePartyAsync(partyInDatabase, cancellationToken);

            AddContributionDTO contriDTO = new AddContributionDTO
            {
                Id = contri.Id
            };

            return Result<AddContributionDTO>.Ok(contriDTO);
        }

        private void AddContribution(Contribution contri)
        {
            AddContributorIfNeeded(contri);
            partyInDatabase.UpdateContributions();
        }

        private void AddContributorIfNeeded(Contribution contri)
        {
            if (!partyInDatabase.Contributions.Any(c => c.Name == contri.Name))
            {
                partyInDatabase.Contributions.Add(contri);
            }
            else
            {
                List<string> existingTags = partyInDatabase.Contributions.First(c => c.Name == contri.Name).Tags;
                existingTags.Concat(contri.Tags);
                partyInDatabase.Contributions.First(c => c.Name == contri.Name).Tags = existingTags.Distinct().ToList();
            }
        }
    }
}
