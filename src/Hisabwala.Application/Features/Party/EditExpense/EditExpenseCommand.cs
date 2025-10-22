using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hisabwala.Application.Features.Party.AddExpense;
using Hisabwala.Core.Common;
using MediatR;

namespace Hisabwala.Application.Features.Party.EditExpense
{
    public class EditExpenseCommand : IRequest<Result<EditExpenseDTO>>
    {
        public string? PartyCode { get; set; }
        public string? ID { get; set; }
        public string? Name { get; set; }
        public decimal Amount { get; set; }
        public string? PaidBy { get; set; }
        public string? Tag { get; set; }
    }
}
