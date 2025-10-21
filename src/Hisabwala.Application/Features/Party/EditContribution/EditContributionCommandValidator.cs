using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Features.Party.AddExpense;
using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
using Hisabwala.Core.Common;

namespace Hisabwala.Application.Features.Party.EditContribution
{
    public class EditContributionCommandValidator : IValidator<EditContributionCommand>
    {
        private readonly IPartyRepository _partyRepository;

        public EditContributionCommandValidator(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<bool>> ValidateAsync(EditContributionCommand request, CancellationToken cancellationToken)
        {
            var partyValidationResult = await Utilities.ValidatePartyCode(request.PartyCode, cancellationToken, _partyRepository);
            if (!partyValidationResult.Success)
                return partyValidationResult;

            if (string.IsNullOrWhiteSpace(request.ID))
                return Result<bool>.Fail("Contribution ID is required.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return Result<bool>.Fail("Contribution Name is required.");

            if (request.Tags.Count == 0)
                return Result<bool>.Fail("At least one tag is required to calculate contribution.");

            if (request.Name.Length > 50)
                return Result<bool>.Fail("Name cannot exceed 50 characters.");

            var partyInfo = await _partyRepository.GetPartyAsync(request.PartyCode, cancellationToken);
            if (request.Tags.Any(tag => !partyInfo.Tags.Contains(tag)))
                return Result<bool>.Fail("One or more invalid tags provided.");

            if (!partyInfo.Contributions.Any(c => c.Id == request.ID))
            {
                return Result<bool>.Fail("Invalid Contribution ID.");
            }

            if (partyInfo.Contributions.Any(c => c.Name.ToLower() == request.Name.ToLower() && c.Id != request.ID))
            {
                return Result<bool>.Fail("Contribution with the same name already exists.");
            }

            return Result<bool>.Ok(true);
        }
    }
}
