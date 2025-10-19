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
            Dictionary<string, decimal> tagAmounts = new();
            foreach (var expense in partyInDatabase.Expenses)
            {
                if (tagAmounts.ContainsKey(expense.Tag))
                {
                    tagAmounts[expense.Tag] += expense.Amount;
                }
                else
                {
                    tagAmounts[expense.Tag] = expense.Amount;
                }
            }

            Dictionary<string, decimal> peoplePerTag = new();
            foreach (var contribution in partyInDatabase.Contributions)
            {
                contribution.Amount = 0;
                foreach (var tag in contribution.Tags.Distinct())
                {
                    if (peoplePerTag.ContainsKey(tag))
                    {
                        peoplePerTag[tag] += 1;
                    }
                    else
                    {
                        peoplePerTag[tag] = 1;
                    }
                }
            }

            foreach (var tagAmount in tagAmounts)
            {
                var tag = tagAmount.Key;
                var totalAmount = tagAmount.Value;
                if (!peoplePerTag.ContainsKey(tag) || peoplePerTag[tag] == 0)
                    continue;
                var peopleCount = peoplePerTag[tag];
                var amountPerPerson = Math.Round(totalAmount / peopleCount);
                foreach (var contribution in partyInDatabase.Contributions)
                {
                    if (contribution.Tags.Contains(tag))
                    {
                        contribution.Amount += amountPerPerson;
                    }
                }
            }
        }
    }
}
