using Hisabwala.Core.Entities;

namespace Hisabwala.Application.Features.Party.GetPartyInformation
{
    public class PartyInformationDTO
    {
        public required string PartyCode { get; set; }
        public required string PartyName { get; set; }
        public required DateTime CreatedDateTime { get; set; }
        public required IReadOnlyList<string> Tags { get; set; }
        public required IReadOnlyList<Expense> Expenses { get; set; }
        public required List<Contribution> Contributions { get; set; }
    }
}
