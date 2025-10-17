using Hisabwala.Core.Entities;

namespace Hisabwala.Application.Features.Party.GetPartyInformation
{
    public class PartyInformationDTO
    {
        public string PartyCode { get; set; } = default!;
        public string PartyName { get; set; } = default!;
        public required DateTime CreatedDateTime { get; set; }
        public required List<Expense> Expenses { get; set; }
        public required List<Contribution> Contributions { get; set; }
    }
}
