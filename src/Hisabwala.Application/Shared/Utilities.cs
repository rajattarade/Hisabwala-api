using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Hisabwala.Application.Interfaces;
using Hisabwala.Core.Common;

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

        public static void UpdateContributions(this Core.Entities.Party partyInDatabase)
        {
            // Reset all contribution amounts
            foreach (var c in partyInDatabase.Contributions)
                c.Amount = 0;

            // Group expenses by tag and calculate total per tag
            var tagTotals = partyInDatabase.Expenses
                .GroupBy(e => e.Tag)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

            // Count how many people have each tag
            var peoplePerTag = partyInDatabase.Contributions
                .SelectMany(c => c.Tags.Distinct(), (c, t) => t)
                .GroupBy(t => t)
                .ToDictionary(g => g.Key, g => g.Count());

            // Distribute tag totals
            foreach (var (tag, total) in tagTotals)
            {
                if (!peoplePerTag.TryGetValue(tag, out var count) || count == 0)
                    continue;

                var share = Math.Round(total / count);
                foreach (var c in partyInDatabase.Contributions.Where(c => c.Tags.Contains(tag)))
                    c.Amount += share;
            }
        }
    }
}
