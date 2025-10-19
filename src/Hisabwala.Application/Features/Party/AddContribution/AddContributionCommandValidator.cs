using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;

namespace Hisabwala.Application.Features.Party.AddContribution
{
    public class AddContributionCommandValidator : IValidator<AddContributionCommand>
    {
        private readonly IPartyRepository _partyRepository;

        public AddContributionCommandValidator(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<bool>> ValidateAsync(AddContributionCommand request, CancellationToken cancellationToken)
        {
            var partyValidationResult = await Utilities.ValidatePartyCode(request.PartyCode, cancellationToken, _partyRepository);
            if (!partyValidationResult.Success)
                return partyValidationResult;

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<bool>.Fail("Name is required.");

            if (request.Tags.Count == 0)
                return Result<bool>.Fail("Atleast one tag is required to calculate contribution.");

            var partyInfo = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);
            if (!partyInfo.Tags.Any(request.Tags.Contains))
                return Result<bool>.Fail("Invalid tag used.");

            return Result<bool>.Ok(true);
        }
    }
}
