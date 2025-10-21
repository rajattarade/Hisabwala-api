using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Features.Party.AddExpense;
using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Party.EditContribution
{
    public class EditContributionCommand : IRequest<Result<EditContributionDTO>>
    {
        public string? PartyCode { get; set; }
        public string? ID { get; set; }
        public string? Name { get; set; }
        public List<string> Tags { get; set; } = default!;
    }
}
