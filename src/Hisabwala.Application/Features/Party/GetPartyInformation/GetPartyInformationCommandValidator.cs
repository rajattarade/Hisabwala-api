using System.Text.RegularExpressions;
using Hisabwala.Application.Interfaces;
using Hisabwala.Application.Shared;
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
            return await Utilities.ValidatePartyCode(request.PartyCode, cancellationToken, _partyRepository);
        }
    }
}
