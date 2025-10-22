namespace Hisabwala.Application.Features.Party.GetPartyInformation
{
    public class PartyInformationDTO
    {
        public required string PartyCode { get; set; }
        public required string PartyName { get; set; }
        public required DateTime CreatedDateTime { get; set; }
        public required IReadOnlyList<string> Tags { get; set; }
        public required IReadOnlyList<Core.Entities.Expense> Expenses { get; set; }
        public required List<Core.Entities.Contribution> Contributions { get; set; }
    }
}
