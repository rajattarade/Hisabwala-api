using Hisabwala.Application.Features.Party;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Entities;
using MongoDB.Driver;

namespace Hisabwala.Infrastructure.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly MongoDbContext _context;

        public PartyRepository(MongoDbContext context)
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

            await _context.Parties.InsertOneAsync(partyEntity);
            return partyEntity;
        }

        public async Task<bool> PartyExistsAsync(string partyCode, CancellationToken cancellationToken)
        {
            return await _context.Parties
                                 .Find(p => p.PartyCode.ToLower() == partyCode.ToLower())
                                 .AnyAsync(cancellationToken);
        }
    }

}
