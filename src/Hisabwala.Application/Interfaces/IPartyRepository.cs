using Hisabwala.Application.Features.Party.GeneratePartyCode;
using Hisabwala.Core.Entities;

namespace Hisabwala.Application.Interfaces
{
    public interface IPartyRepository
    {
        Task<bool> PartyExistsAsync(string partyCode, CancellationToken cancellationToken);
        Task<Party> AddPartyAsync(PartyCodeDTO dto, CancellationToken cancellationToken);
        Task<Party> GetPartyAsync(string partyCode, CancellationToken cancellationToken);
    }
}
