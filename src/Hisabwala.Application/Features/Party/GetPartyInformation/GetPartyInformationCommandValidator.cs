using System.Text.RegularExpressions;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Common;

namespace Hisabwala.Application.Features.Party.GetPartyInformation
{
    public class GetPartyInformationCommandValidator : IValidator<GetPartyInformationCommand>
    {
        private readonly IPartyRepository _partyRepository;

        public GetPartyInformationCommandValidator(IPartyRepository partyRepository) 
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<bool>> ValidateAsync(GetPartyInformationCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.PartyCode))
                return Result<bool>.Fail("Party code is required.");

            if (request.PartyCode.Length != 9)
                return Result<bool>.Fail("Party code must be 9 characters.");

            if (!Regex.IsMatch(request.PartyCode, @"^[a-zA-Z0-9-]+$"))
                return Result<bool>.Fail("Party name must only contain letters, numbers and, hyphen (-).");

            if (!await _partyRepository.PartyExistsAsync(request.PartyCode, cancellationToken))
                return Result<bool>.Fail("Party with the given code does not exist.");
            
            return Result<bool>.Ok(true);
        }
    }
}
