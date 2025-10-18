using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Shared
{
    public static class Utilities
    {
        public static async Task<Result<bool>> ValidatePartyCode(string partyCode, CancellationToken cancellationToken, IPartyRepository _partyRepository)
        {
            if (string.IsNullOrWhiteSpace(partyCode))
                return Result<bool>.Fail("Party code is required.");

            if (partyCode.Length != 9)
                return Result<bool>.Fail("Party code must be 9 characters.");

            if (!Regex.IsMatch(partyCode, @"^[a-zA-Z0-9-]+$"))
                return Result<bool>.Fail("Party name must only contain letters, numbers and, hyphen (-).");

            if (!await _partyRepository.PartyExistsAsync(partyCode, cancellationToken))
                return Result<bool>.Fail("Party with the given code does not exist.");

            return Result<bool>.Ok(true);
        }
    }
}
