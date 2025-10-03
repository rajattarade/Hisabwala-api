using Hisabwala.Application.Features.Party;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Entities;
using Hisabwala.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hisabwala.Infrastructure.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly AppDbContext _context;

        public PartyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Party> AddPartyAsync(PartyCodeDTO dto, CancellationToken cancellationToken)
        {
            var partyEntity = new Party
            {
                PartyName = dto.PartyName,
                PartyCode = dto.PartyCode
            };

            _context.Parties.Add(partyEntity);
            await _context.SaveChangesAsync(cancellationToken);
            return partyEntity;
        }

        public async Task<bool> PartyExistsAsync(string partyCode, CancellationToken cancellationToken)
        {
            return await _context.Parties.AnyAsync(p => p.PartyCode.ToLower() == partyCode.ToLower(), cancellationToken);
        }
    }

}
