using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;
using Hisabwala.Core.Entities;
using MediatR;
using MongoDB.Bson;

namespace Hisabwala.Application.Features.Party.AddContribution
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

            partyInDatabase.Contributions.Add(contri);
            partyInDatabase.UpdateContributions();

            await _partyRepository.UpdatePartyAsync(partyInDatabase, cancellationToken);

            AddContributionDTO contriDTO = new AddContributionDTO
            {
                Id = contri.Id
            };

            return Result<AddContributionDTO>.Ok(contriDTO);
        }
    }
}
