using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Contribution.DeleteContribution
{
    public class DeleteContributionCommand : IRequest<Result<DeleteContributionDTO>>
    {
        public string? PartyCode { get; set; }
        public string? ID { get; set; }
    }
}
