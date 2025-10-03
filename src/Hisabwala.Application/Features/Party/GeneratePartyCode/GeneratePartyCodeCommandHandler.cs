using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Features.Tags.AddTag;
using Hisabwala.Application.Features.Tags;
using Hisabwala.Core.Common;
using MediatR;
using Hisabwala.Application.Interfaces;

namespace Hisabwala.Application.Features.Party.GeneratePartyCode
{
    public class GeneratePartyCodeCommandHandler : IRequestHandler<GeneratePartyCodeCommand, Result<PartyCodeDTO>>
    {
        private readonly IPartyRepository _partyRepository;
        private static readonly Random _random = new();

        public GeneratePartyCodeCommandHandler(IPartyRepository partyRepository)
        {
            _partyRepository = partyRepository;
        }

        public async Task<Result<PartyCodeDTO>> Handle(GeneratePartyCodeCommand request, CancellationToken cancellationToken)
        {
            // Take first 4 alphanumeric chars from PartyName
            string prefix = new string(request.PartyName
                                              .Where(char.IsAsciiLetterOrDigit)
                                              .Take(4)
                                              .ToArray())
                                              .ToUpper();

            const int MaxAttempts = 10;

            for (int attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                var randomPart = GenerateRandomAlphanumeric(4);
                var partyCode = $"{prefix}-{randomPart}";

                // if already exists, retry
                if (await _partyRepository.PartyExistsAsync(partyCode, cancellationToken))
                    continue;

                // persist
                var partyDTO = new PartyCodeDTO
                {
                    PartyName = request.PartyName,
                    PartyCode = partyCode
                };

                var savedParty = await _partyRepository.AddPartyAsync(partyDTO, cancellationToken);

                var dto = new PartyCodeDTO
                {
                    Id = savedParty.Id,
                    PartyName = savedParty.PartyName,
                    PartyCode = savedParty.PartyCode,
                    CreatedDateTime = savedParty.CreatedDateTime
                };

                return Result<PartyCodeDTO>.Ok(dto);
            }

            return Result<PartyCodeDTO>.Fail("Unable to generate a unique party code. Please try again.");
        }

        private static string GenerateRandomAlphanumeric(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Range(0, length)
                .Select(_ => chars[_random.Next(chars.Length)])
                .ToArray());
        }
    }
}
