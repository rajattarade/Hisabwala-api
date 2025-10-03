using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Features.Party;
using Hisabwala.Core.Entities;

namespace Hisabwala.Application.Interfaces
{
    public interface IPartyRepository
    {
        Task<bool> PartyExistsAsync(string partyCode, CancellationToken cancellationToken);
        Task<Party> AddPartyAsync(PartyCodeDTO dto, CancellationToken cancellationToken);
    }
}
