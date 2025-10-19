using Hisabwala.Application.Features.Party.AddExpense;
using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Party.AddContribution
{
    public class AddContributionCommand : IRequest<Result<AddContributionDTO>>
    {
        public string PartyCode { get; set; } = null!;
        public string Name { get; set; } = default!;
        public List<string> Tags { get; set; } = default!;
    }
}
