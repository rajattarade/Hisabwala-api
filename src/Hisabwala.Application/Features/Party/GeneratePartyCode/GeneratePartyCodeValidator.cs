using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hisabwala.Core.Common;

namespace Hisabwala.Application.Features.Party.GeneratePartyCode
{
    public class GeneratePartyCodeValidator : IValidator<GeneratePartyCodeCommand>
    {
        public GeneratePartyCodeValidator() { }
        public Task<Result<bool>> ValidateAsync(GeneratePartyCodeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.PartyName))
                return Task.FromResult(Result<bool>.Fail("Party name is required."));

            if (request.PartyName.Where(char.IsAsciiLetterOrDigit).ToList().Count < 4 || request.PartyName.Length > 100)
                return Task.FromResult(Result<bool>.Fail("Party name must be between 4 and 100 characters."));

            if (!Regex.IsMatch(request.PartyName, @"^[a-zA-Z0-9\s]+$"))
                return Task.FromResult(Result<bool>.Fail("Party name must only contain letters, numbers and, spaces without special characters."));

            return Task.FromResult(Result<bool>.Ok(true));
        }
    }
}
