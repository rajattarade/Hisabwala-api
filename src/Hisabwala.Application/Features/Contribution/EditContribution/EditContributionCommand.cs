using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Contribution.EditContribution
{
    public class EditContributionCommand : IRequest<Result<EditContributionDTO>>
    {
        public string? PartyCode { get; set; }
        public string? ID { get; set; }
        public string? Name { get; set; }
        public List<string> Tags { get; set; } = default!;
    }
}
