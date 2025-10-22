using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;

namespace Hisabwala.Application.Features.Contribution.DeleteContribution
{
    public class DeleteContributionCommandValidator : IValidator<DeleteContributionCommand>
    {
        private readonly IPartyRepository _partyRepository;

        public DeleteContributionCommandValidator(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<bool>> ValidateAsync(DeleteContributionCommand request, CancellationToken cancellationToken)
        {
            var partyValidationResult = await Utilities.ValidatePartyCode(request.PartyCode!, cancellationToken, _partyRepository);
            if (!partyValidationResult.Success)
                return partyValidationResult;

            if (string.IsNullOrWhiteSpace(request.ID))
                return Result<bool>.Fail("Contribution ID is required.");

            var partyInfo = await _partyRepository.GetPartyAsync(request.PartyCode!, cancellationToken);            
            if (!partyInfo.Contributions.Any(c => c.Id == request.ID))
            {
                return Result<bool>.Fail("Invalid Contribution ID.");
            }

            return Result<bool>.Ok(true);
        }
    }
}
